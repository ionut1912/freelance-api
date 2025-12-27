using Freelance.UserProfiles.Api.Endpoints;
using Freelance.UserProfiles.Api.Mappers;
using Freelance.UserProfiles.Application.Mediatr;
using Freelance.UserProfiles.Application.Validators;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Freelance.UserProfiles.Infrastructure.Persistance.Repository;
using Shared.Api.Extensions;
using Shared.Domain.Interfaces;
using Shared.Infra.Services;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://tempo:4317";
var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-User-Profiles";
var environmentName = builder.Environment.EnvironmentName ?? "Development";

// Use the correct Loki OTLP endpoint
var lokiEndpoint = configuration["OTEL_EXPORTER_OTLP_LOGS_ENDPOINT"] ?? "http://loki:3100";

var resourceBuilder = OpenTelemetryExtensions.CreateServiceResourceBuilder(serviceName, environmentName);

builder.AddOpenTelemetry(lokiEndpoint, resourceBuilder);

builder.Services
    .AddDatabaseConfig<ApplicationDbContext>(builder.Configuration)
    .AddRepository<ClientProfile, ClientProfileService, IClientProfileRepository, ApplicationDbContext>()
    .AddRepository<FreelancerProfile,FreelancerProfileService, IFreelancerProfileRepository, ApplicationDbContext>()
    .AddRepositoriesConfig<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>()
    .AddAplicationConfig(typeof(MediatrAssemblyReference).Assembly, typeof(ValidationAssemblyReference).Assembly)
    .AddPresentation<UserProfileExceptionMapper>(builder.Configuration, otelEndpoint, serviceName, environmentName);

var app = builder.Build();

app.MigrateDatabaseConfig<ApplicationDbContext>();

app.UseGlobalExceptionHandler<Program>()
    .UseRequestDurationLogging<Program>()
    .UseStandardMiddleware()
    .MapStandardEndpoints();

app.MapApiDocumentation();
app.MapUserProfileEndpoints();

app.Logger.LogInformation("🚀 {ServiceName} starting up in {Environment} environment", serviceName, environmentName);

app.Run();