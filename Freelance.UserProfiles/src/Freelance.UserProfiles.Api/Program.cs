using Freelance.UserProfiles.Api.Handlers;
using Freelance.UserProfiles.Api.Modules;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelance.UserProfiles.Infrastructure.Persistance;
using Freelance.UserProfiles.Infrastructure.Persistance.Repository;
using Freelancer.UserProfiles.Application.Mappings;
using Freelancer.UserProfiles.Application.Mediatr;
using Freelancer.UserProfiles.Application.Validators;
using Microsoft.AspNetCore.Diagnostics;
using RabbitMQ.Client;
using Shared.Api.Extensions;
using Shared.Domain.Interfaces;
using Shared.Infra.Extensions;
using Shared.Infra.Services;
using Shared.Rabbit.Extensions;
using Shared.Rabbit.Settings;
using ServiceCollectionExtensions = Shared.Api.Extensions.ServiceCollectionExtensions;


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

var factory = new ConnectionFactory
{
    HostName = builder.Configuration["RabbitMQ:HostName"],
    UserName = builder.Configuration["RabbitMQ:UserName"],
    Password = builder.Configuration["RabbitMQ:Password"]
};

var connection = factory.CreateConnection();
builder.Services.AddSingleton(connection);
builder.Services.AddRabbitMqEventBus(options =>
{
    options.ExchangeConfigurations.Add(new ExchangeConfiguration
    {
        Name = "userprofile.events",
        Type = ExchangeType.Topic,
        Durable = true
    });

    options.ExchangeConfigurations.Add(new ExchangeConfiguration
    {
        Name = "faceverification.events",
        Type = ExchangeType.Topic,
        Durable = true
    });

    options.ExchangeResolver = eventName =>
    {
        if (eventName.StartsWith("FaceVerification"))
            return "faceverification.events";

        return "userprofile.events";
    };

    options.RoutingKeyResolver = eventName =>
    {
        return eventName.ToLowerInvariant().Replace("event", "");
    };
});
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