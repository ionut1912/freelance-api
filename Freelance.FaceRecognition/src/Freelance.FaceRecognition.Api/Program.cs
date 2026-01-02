using Freelance.FaceRecognition.Api.Endpoints;
using Freelance.FaceRecognition.Api.Mappers;
using Freelance.FaceRecognition.Application.EventHandlers;
using Freelance.FaceRecognition.Domain.Interfaces;
using Freelance.FaceRecognition.Infrastructure.Hubs;
using Freelance.FaceRecognition.Infrastructure.Services;
using Freelance.Shared.Events.Events;
using Shared.Api.Extensions;
using Shared.Api.Infrastructure;
using Shared.Rabbit.Extensions;
using Shared.Rabbit.Repositories;
using Shared.Rabbit.Settings;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://tempo:4317";
var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-Face-Recognition";
var environmentName = builder.Environment.EnvironmentName ?? "Development";

// Use the correct Loki OTLP endpoint
var lokiEndpoint = configuration["OTEL_EXPORTER_OTLP_LOGS_ENDPOINT"] ?? "http://loki:3100";

var resourceBuilder = OpenTelemetryExtensions.CreateServiceResourceBuilder(serviceName, environmentName);
var rabbitMqSection = builder.Configuration.GetSection("RabbitMq");


builder.Services.AddRabbitMqEventBus(
    options =>
    {
        options.ExchangeConfigurations = rabbitMqSection
            .GetSection("Exchanges")
            .Get<List<ExchangeConfiguration>>();
    },
    hostname: rabbitMqSection["HostName"]!,
    username: rabbitMqSection["UserName"]!,
    password: rabbitMqSection["Password"]!
);

builder.Services.AddSignalR(options => { options.MaximumReceiveMessageSize = 10 * 1024 * 1024; });
builder.AddOpenTelemetry(lokiEndpoint, resourceBuilder);

builder.Services
    .AddRepositoriesConfig<IFaceComparisonRepository, FaceVerifcationService>()
    .AddPresentation<FaceRecognitionExceptionMapper>(builder.Configuration, otelEndpoint, serviceName, environmentName);

builder.Services.AddScoped<VerifyFaceEventHandler>();
var app = builder.Build();

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<VerifyFaceEvent, VerifyFaceEventHandler>();
app.UseGlobalExceptionHandler<Program>()
    .UseRequestDurationLogging<Program>()
    .UseStandardMiddleware()
    .MapStandardEndpoints();

app.MapApiDocumentation();
app.MapHub<CaptureHub>("/hubs/capture");
app.MapEndpoints(typeof(CameraEndpointsGroup).Assembly);
app.Logger.LogInformation("🚀 {ServiceName} starting up in {Environment} environment", serviceName, environmentName);

app.Run();