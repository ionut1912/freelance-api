using Freelance.ProjectManagement.Api.Endpoints;
using Freelance.ProjectManagement.Api.Mappers;
using Freelance.ProjectManagement.Application.Mediatr;
using Freelance.ProjectManagement.Application.Validators;
using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Interfaces;
using Freelance.ProjectManagement.Infrastructure.Persistance;
using Freelance.ProjectManagement.Infrastructure.Persistance.Repositories;
using Shared.Api.Extensions;
using Shared.Api.Infrastructure;
using Shared.Domain.Interfaces;
using Shared.Infra.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://tempo:4317";
var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-Project-Management";
var environmentName = builder.Environment.EnvironmentName ?? "Development";

// Use the correct Loki OTLP endpoint
var lokiEndpoint = configuration["OTEL_EXPORTER_OTLP_LOGS_ENDPOINT"] ?? "http://loki:3100";

var resourceBuilder = OpenTelemetryExtensions.CreateServiceResourceBuilder(serviceName, environmentName);

builder.AddOpenTelemetry(lokiEndpoint, resourceBuilder);

builder.Services
    .AddDatabaseConfig<ApplicationDbContext>(builder.Configuration)
    .AddRepository<Project, ProjectRepository, IProjectRepository, ApplicationDbContext>()
    .AddRepository<ProjectTask, ProjectTaskRepository, IProjectTaskRepository, ApplicationDbContext>()
    .AddRepository<TimeLog, TimeLogRepository, ITimeLogRepository, ApplicationDbContext>()
    .AddRepositoriesConfig<IUnitOfWork, UnitOfWork>()
    .AddAplicationConfig(typeof(MediatrAssemblyReference).Assembly, typeof(ValidatorAssemblyReference).Assembly)
    .AddPresentation<ProjectManagementMapper>(builder.Configuration, otelEndpoint, serviceName, environmentName);


var app = builder.Build();
app.MigrateDatabaseConfig<ApplicationDbContext>();

app.UseGlobalExceptionHandler<Program>()
    .UseRequestDurationLogging<Program>()
    .UseStandardMiddleware()
    .MapStandardEndpoints();

app.MapApiDocumentation();
app.MapEndpoints(typeof(ProjectEndpointGroup).Assembly);

app.Logger.LogInformation("🚀 {ServiceName} starting up in {Environment} environment", serviceName, environmentName);

app.Run();