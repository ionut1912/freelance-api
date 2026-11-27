using Freelance.ProjectManagement.Api;
using Freelance.ProjectManagement.Api.Endpoints;
using Freelance.ProjectManagement.Api.Mappers;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Infrastructure;
using Freelance.ProjectManagement.Infrastructure.Persistance;
using JasperFx.CodeGeneration.Model;
using Shared.Api.Extensions;
using Shared.Api.Infrastructure;
using Shared.Infra.Extensions;
using Wolverine;
using Wolverine.FluentValidation;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-Project-Management";
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://tempo:4317";
var lokiEndpoint = configuration["OTEL_EXPORTER_OTLP_LOGS_ENDPOINT"] ?? "http://loki:3100";

builder.AddOpenTelemetry(lokiEndpoint,
    OpenTelemetryExtensions.CreateServiceResourceBuilder(serviceName, builder.Environment.EnvironmentName));

builder.Services
    .AddProjectManagementInfrastructure(configuration)
    .AddPresentation<ProjectManagementMapper>(configuration, serviceName, otelEndpoint,
        needsRoles: true,
        requiredPolicies: AuthorizationPolicies.Policies,
        requiredRoles: AuthorizationPolicies.Roles);

// Wolverine is the in-process mediator: handlers are discovered by convention in the Application
// assembly and FluentValidation validators run as middleware in front of every handler.
builder.Host.UseWolverine(options =>
{
    options.Discovery.IncludeAssembly(typeof(CreateProjectCommand).Assembly);
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

app.MapEndpoints(typeof(ProjectEndpointGroup).Assembly);

app.Logger.LogInformation("{ServiceName} starting up in {Environment} environment", serviceName,
    app.Environment.EnvironmentName);

await app.RunAsync();
