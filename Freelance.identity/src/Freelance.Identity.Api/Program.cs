using Freelance.Identity.Api.Endpoints;
using Freelance.Identity.Api.Mappers;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Validators;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Infrastructure.Persistance;
using Freelance.Identity.Infrastructure.Persistance.Repositories;
using Shared.Api.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://alloy:4317";
var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-Identity";
var environmentName = builder.Environment.EnvironmentName ?? "Development";

// Create OpenTelemetry resource
builder.AddOpenTelemetry(otelEndpoint, serviceName, environmentName);


builder.Services
    .AddDatabaseConfig<ApplicationDbContext>(builder.Configuration)
    .AddRepositoriesConfig<IAccountRepository, AccountRepository>()
    .AddRepositoriesConfig<IJwtTokenService, JwtTokenService>()
    .AddRepositoriesConfig<IPasswordService, PasswordService>()
    .AddAplicationConfig(typeof(CreateAccountCommand).Assembly, typeof(CreateAccountCommandValidator).Assembly)
    .AddPresentation<IdentityExceptionMapper>(builder.Configuration, otelEndpoint, serviceName, environmentName);


var app = builder.Build();

// Migrate database
app.MigrateDatabaseConfig<ApplicationDbContext>();

// Configure middleware pipeline
app.UseGlobalExceptionHandler<Program>()
    .UseRequestDurationLogging<Program>()
    .UseStandardMiddleware()
    .MapStandardEndpoints();

app.MapApiDocumentation();

app.MapUsersEndpoints();

app.Run();