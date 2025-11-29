using Freelance.Identity.Api.Handlers;
using Freelance.Identity.Api.Modules;
using Freelance.Identity.Application.Mappings;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Validators;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Infrastructure.Persistance;
using Freelance.Identity.Infrastructure.Persistance.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Shared.Api.Extensions;
using Shared.Domain.Interfaces;
using Shared.Infra.Extensions;
using Shared.Infra.Services;
using ServiceCollectionExtensions = Shared.Api.Extensions.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://alloy:4317";
var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-Identity";
var environmentName = builder.Environment.EnvironmentName ?? "Development";

// Create OpenTelemetry resource
var resourceBuilder = ServiceCollectionExtensions.CreateServiceResourceBuilder(
    serviceName, 
    environmentName);

// Register services
builder.Services
    .AddDatabaseContext<ApplicationDbContext>(configuration)
    .AddMediatorWithValidation(
        typeof(CreateAccountCommand),
        typeof(CreateAccountCommandValidator),
        typeof(MappingProfile))
    .AddJwtAuthentication(configuration)
    .AddRoleBasedAuthorization()
    .AddOpenTelemetryObservability(otelEndpoint, serviceName, resourceBuilder)
    .AddSwaggerWithJwtAuth("Freelance Identity API");

// Logging
builder.Logging.AddOpenTelemetryLogging(otelEndpoint, resourceBuilder);

// Domain-specific services
builder.Services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
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
app.AddUserEndpoints();

app.Run();