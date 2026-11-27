using Freelance.Face.Api.Endpoints;
using Freelance.Face.Application.EventHandlers;
using Freelance.Face.Infrastructure;
using Freelance.Face.Api.Hubs;
using Freelance.Face.Api.Options;
using Freelance.Shared.Events.Events;
using Microsoft.AspNetCore.Diagnostics;
using Shared.Api.Extensions;
using Shared.Api.Handlers;
using Shared.Api.Infrastructure;
using Shared.Application.Options;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-Face-Recognition";
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://tempo:4317";
var lokiEndpoint = configuration["OTEL_EXPORTER_OTLP_LOGS_ENDPOINT"] ?? "http://loki:3100";

builder.AddOpenTelemetry(lokiEndpoint,
    OpenTelemetryExtensions.CreateServiceResourceBuilder(serviceName, builder.Environment.EnvironmentName));

// This service has no database, so it composes the Shared.Api building blocks it needs instead of
// AddPresentation (which also registers JWT auth and a PostgreSQL health check).
builder.Services
    .AddOpenTelemetryObservability(otelEndpoint, serviceName)
    .AddSingleton<IExceptionHandler, GlobalExceptionHandler>()
    .AddJwtAuthentication(configuration)
    .AddAuthorization()
    .AddFaceInfrastructure()
    .AddOpenApi()
    .AddHealthChecks();

// Captured photos travel through the hub as data URLs.
const long MaxHubMessageBytes = 10 * 1024 * 1024;
builder.Services.AddSignalR(options => options.MaximumReceiveMessageSize = MaxHubMessageBytes);
builder.Services.Configure<FrontendOptions>(configuration);

builder.Services.AddOptions<RabbitMqOptions>()
    .BindConfiguration(RabbitMqOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Wolverine over RabbitMQ without message persistence (stateless service): consume verification requests,
// reply with the result as a cascading message.
builder.Host.UseWolverine(options =>
{
    options.Discovery.IncludeAssembly(typeof(FaceVerificationRequestHandler).Assembly);

    var rabbit = configuration.GetSection(RabbitMqOptions.SectionName).Get<RabbitMqOptions>() ?? new RabbitMqOptions();
    options.UseRabbitMq(factory =>
    {
        factory.HostName = rabbit.Host;
        factory.Port = rabbit.Port;
        factory.UserName = rabbit.Username;
        factory.Password = rabbit.Password;
    }).AutoProvision();

    options.ListenToRabbitQueue(QueueNames.FaceVerificationRequests);
    options.PublishMessage<VerifiedFaceEvent>().ToRabbitQueue(QueueNames.FaceVerificationResults);
});

var app = builder.Build();

app.UseGlobalExceptionHandler<Program>()
    .UseRequestDurationLogging<Program>()
    .UseRouting()
    .MapStandardEndpoints()
    .MapApiDocumentation();

app.UseAuthentication();
app.UseAuthorization();

// The hub stays anonymous: the phone opening the deep link has no login, only the unguessable session id.
app.MapHub<CaptureHub>("/hubs/capture");
app.MapEndpoints(typeof(CameraEndpointGroup).Assembly);

app.Logger.LogInformation("{ServiceName} starting up in {Environment} environment", serviceName,
    app.Environment.EnvironmentName);

await app.RunAsync();
