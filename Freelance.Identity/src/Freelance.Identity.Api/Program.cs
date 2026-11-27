using Freelance.Identity.Api.Endpoints;
using Freelance.Identity.Api.Mappers;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Infrastructure;
using Freelance.Identity.Infrastructure.Persistance;
using JasperFx.CodeGeneration.Model;
using Shared.Api.Extensions;
using Shared.Api.Infrastructure;
using Shared.Infra.Extensions;
using Wolverine;
using Wolverine.FluentValidation;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-Identity";
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://tempo:4317";
var lokiEndpoint = configuration["OTEL_EXPORTER_OTLP_LOGS_ENDPOINT"] ?? "http://loki:3100";

builder.AddOpenTelemetry(lokiEndpoint,
    OpenTelemetryExtensions.CreateServiceResourceBuilder(serviceName, builder.Environment.EnvironmentName));

builder.Services
    .AddIdentityInfrastructure(configuration)
    .AddPresentation<IdentityExceptionMapper>(configuration, serviceName, otelEndpoint)
    .AddAuthorization();

// Wolverine is the in-process mediator: handlers are discovered by convention in the Application
// assembly and FluentValidation validators run as middleware in front of every handler.
builder.Host.UseWolverine(options =>
{
    options.Discovery.IncludeAssembly(typeof(CreateAccountCommand).Assembly);
    options.UseFluentValidation();

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

app.MapEndpoints(typeof(UserEndpointGroup).Assembly);

app.Logger.LogInformation("{ServiceName} starting up in {Environment} environment", serviceName,
    app.Environment.EnvironmentName);

await app.RunAsync();
