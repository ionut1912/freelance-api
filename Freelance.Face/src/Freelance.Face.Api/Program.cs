using Freelance.Face.Api.Endpoints;
using Freelance.Face.Application.EventHandlers;
using Freelance.Face.Domain.Interfaces;
using Freelance.Face.Infrastructure.Hubs;
using Freelance.Face.Infrastructure.Services;
using Freelance.Shared.Events.Events;
using Microsoft.AspNetCore.Diagnostics;
using Shared.Api.Extensions;
using Shared.Api.Handlers;
using Shared.Api.Infrastructure;
using Shared.Rabbit.Extensions;
using Shared.Rabbit.Repositories;
using Shared.Rabbit.Settings;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://tempo:4317";
var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-Face-Recognition";
var environmentName = builder.Environment.EnvironmentName ?? "Development";
var lokiEndpoint = configuration["OTEL_EXPORTER_OTLP_LOGS_ENDPOINT"] ?? "http://loki:3100";

var resourceBuilder = OpenTelemetryExtensions.CreateServiceResourceBuilder(serviceName, environmentName);

var rabbitMqSection = configuration.GetSection("RabbitMq");
var exchanges = rabbitMqSection.GetSection("Exchanges").Get<List<ExchangeConfiguration>>() ?? new();

builder.Services.AddRabbitMqEventBus(
    options => { options.ExchangeConfigurations = exchanges; },
    hostname: rabbitMqSection["HostName"] ?? "rabbitmq",
    username: rabbitMqSection["UserName"] ?? "guest",
    password: rabbitMqSection["Password"] ?? "guest"
);

builder.Services.AddSignalR(o => { o.MaximumReceiveMessageSize = 10 * 1024 * 1024; });

builder.AddOpenTelemetry(lokiEndpoint, resourceBuilder);

builder.Services
    .AddOpenTelemetryObservability(otelEndpoint, serviceName)
    .AddSingleton<IExceptionHandler, GlobalExceptionHandler>();

builder.Services.AddSingleton<IFaceService, FaceService>();

builder.Services.AddTransient<VerifyFaceEventHandler>();

builder.Services.AddRouting();
builder.Services.AddHealthChecks();
var app = builder.Build();

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<VerifyFaceEvent, VerifyFaceEventHandler>();

app.UseGlobalExceptionHandler<Program>()
   .UseRequestDurationLogging<Program>()
   .UseStandardMiddleware()
   .MapStandardEndpoints();

app.MapApiDocumentation();
app.MapHub<CaptureHub>("/hubs/capture");
app.MapEndpoints(typeof(CameraEndpointGroup).Assembly);

app.Logger.LogInformation("🚀 {ServiceName} starting up in {Environment} environment", serviceName, environmentName);

app.Run();
