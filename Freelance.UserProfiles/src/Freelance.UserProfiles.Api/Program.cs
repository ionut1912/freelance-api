using Freelance.UserProfiles.Api.Endpoints;
using Freelance.UserProfiles.Api.Mappers;
using Freelance.UserProfiles.Application.Mediatr;
using Freelance.UserProfiles.Application.Validators;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Freelance.UserProfiles.Infrastructure.Persistance.Repository;
using Shared.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://alloy:4317";
var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-User-Profiles";
var environmentName = builder.Environment.EnvironmentName ?? "Development";

// Create OpenTelemetry resource
builder.AddOpenTelemetry(otelEndpoint, serviceName, environmentName);


builder.Services
    .AddDatabaseConfig<ApplicationDbContext>(builder.Configuration)
    .AddRepositoriesConfig<IFreelancerProfileRepository, FreelancerProfileService>()
    .AddRepositoriesConfig<IClientProfileRepository, ClientProfileService>()
    .AddAplicationConfig(typeof(MediatrAssemblyReference).Assembly, typeof(ValidationAssemblyReference).Assembly)
    .AddPresentation<UserProfileExceptionMapper>(builder.Configuration, otelEndpoint, serviceName, environmentName);


var app = builder.Build();

// Migrate database
app.MigrateDatabaseConfig<ApplicationDbContext>();

// Configure middleware pipeline
app.UseGlobalExceptionHandler<Program>()
    .UseRequestDurationLogging<Program>()
    .UseStandardMiddleware()
    .MapStandardEndpoints();

app.MapApiDocumentation();

app.MapUserProfileEndpoints();

app.Run();