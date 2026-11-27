using Freelance.Shared.Events.Events;
using Freelance.UserProfiles.Api;
using Freelance.UserProfiles.Api.Endpoints;
using Freelance.UserProfiles.Api.Mappers;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Infrastructure;
using Freelance.UserProfiles.Infrastructure.Persistance;
using JasperFx.CodeGeneration.Model;
using Shared.Api.Extensions;
using Shared.Api.Infrastructure;
using Shared.Infra.Extensions;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-User-Profiles";
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://tempo:4317";
var lokiEndpoint = configuration["OTEL_EXPORTER_OTLP_LOGS_ENDPOINT"] ?? "http://loki:3100";

builder.AddOpenTelemetry(lokiEndpoint,
    OpenTelemetryExtensions.CreateServiceResourceBuilder(serviceName, builder.Environment.EnvironmentName));

builder.Services
    .AddUserProfilesInfrastructure(configuration)
    .AddPresentation<UserProfileExceptionMapper>(configuration, serviceName, otelEndpoint,
        needsRoles: true,
        requiredPolicies: AuthorizationPolicies.Policies,
        requiredRoles: AuthorizationPolicies.Roles);

// Wolverine: in-process mediator + RabbitMQ transport with a PostgreSQL-backed durable outbox/inbox
// (configured by Shared.Api). Here we only declare this service's routing.
builder.AddWolverineMessaging(typeof(CreateClientProfileCommand).Assembly, static (_, options) =>
{
    options.PublishMessage<VerifyFaceEvent>().ToRabbitQueue(QueueNames.FaceVerificationRequests);
    options.ListenToRabbitQueue(QueueNames.FaceVerificationResults);

    // Shared.Infra registers repositories and the DbContext alias through lambda factories, which Wolverine
    // cannot inline into generated handler code; resolve those from the scope instead of failing.
    options.ServiceLocationPolicy = ServiceLocationPolicy.AllowedButWarn;
});

var app = builder.Build();

app.MigrateServiceDatabase<ApplicationDbContext>();

app.UseGlobalExceptionHandler<Program>()
    .UseRequestDurationLogging<Program>()
    .UseStandardMiddleware()
    .MapStandardEndpoints()
    .MapApiDocumentation();

app.MapEndpoints(typeof(ClientProfileEndpointGroup).Assembly);

app.Logger.LogInformation("{ServiceName} starting up in {Environment} environment", serviceName,
    app.Environment.EnvironmentName);

await app.RunAsync();
