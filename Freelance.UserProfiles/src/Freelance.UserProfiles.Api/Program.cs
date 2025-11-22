using Freelance.Shared.Domain.Interfaces;
using Freelance.Shared.Infrastructure.Extensions;
using Freelance.Shared.Infrastructure.Services;
using Freelance.UserProfiles.Api.Handlers;
using Freelance.UserProfiles.Api.Modules;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Freelance.UserProfiles.Infrastructure.Persistance.Repository;
using Freelancer.UserProfiles.Application.Mappings;
using Freelancer.UserProfiles.Application.Mediatr;
using Freelancer.UserProfiles.Application.Validators;
using Microsoft.AspNetCore.Diagnostics;
using Freelance.Shared.Api.Extensions;
using ServiceCollectionExtensions = Freelance.Shared.Api.Extensions.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://alloy:4317";
var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-UserProfiles";
var environmentName = builder.Environment.EnvironmentName ?? "Development";

// Create OpenTelemetry resource
var resourceBuilder = ServiceCollectionExtensions.CreateServiceResourceBuilder(
    serviceName, 
    environmentName);

// Register services
builder.Services
    .AddDatabaseContext<ApplicationDbContext>(configuration)
    .AddMediatorWithValidation(
        typeof(MediatrAssemblyReference),
        typeof(ValidationAssemblyReference),
        typeof(MappingProfile))
    .AddJwtAuthentication(configuration)
    .AddRoleBasedAuthorization()
    .AddOpenTelemetryObservability(otelEndpoint, serviceName, resourceBuilder)
    .AddSwaggerWithJwtAuth("Freelance UserProfiles API");

// Logging
builder.Logging.AddOpenTelemetryLogging(otelEndpoint, resourceBuilder);

// Domain-specific services
builder.Services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();
builder.Services.AddScoped<IClientProfileRepository, ClientProfileService>();
builder.Services.AddScoped<IFreelancerProfileRepository, FreelancerProfileService>();
builder.Services.AddSingleton<IExceptionHandler, ExceptionHandler>();

// Health checks and controllers
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

// Migrate database
app.Services.MigrateDatabase<ApplicationDbContext>();

// Configure middleware pipeline
app.UseGlobalExceptionHandler<Program>()
    .UseRequestDurationLogging<Program>()
    .UseStandardMiddleware()
    .MapStandardEndpoints();

// Map domain endpoints
app.AddClientProfileEndpoints();
app.AddFreelancerProfileEndpoints();

app.Run();