using Freelance.Identity.Api.Modules;
using Shared.Api.Extensions;
using Freelance.Identity.Infrastructure.Extensions;
using Freelance.Identity.Application.Extensions;
using Freelance.Identity.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://alloy:4317";
var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-Identity";
var environmentName = builder.Environment.EnvironmentName ?? "Development";

// Create OpenTelemetry resource
var resourceBuilder = Shared.Api.Extensions.ServiceCollectionExtensions.CreateServiceResourceBuilder(
    serviceName,
    environmentName);
// Logging
builder.Logging.AddOpenTelemetryLogging(otelEndpoint, resourceBuilder);

// Register layers
builder.Services
    .AddInfrastructure(configuration)
    .AddApplication()
    .AddPresentation(configuration, otelEndpoint, resourceBuilder);




var app = builder.Build();

// Migrate database
app.MigrateIdentityDatabase();

// Configure middleware pipeline
app.UseGlobalExceptionHandler<Program>()
    .UseRequestDurationLogging<Program>()
    .UseStandardMiddleware()
    .MapStandardEndpoints();

app.MapApiDocumentation();

// Map domain endpoints
app.AddUserEndpoints();

app.Run();