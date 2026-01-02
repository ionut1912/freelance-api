using Freelance.Identity.Api.Endpoints;
using Freelance.Identity.Api.Mappers;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Validators;
using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Infrastructure.Persistance;
using Freelance.Identity.Infrastructure.Persistance.Repositories;
using Shared.Api.Extensions;
using Shared.Api.Infrastructure;
using Shared.Domain.Interfaces;
using Shared.Infra.Services;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://tempo:4317";
var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-Identity";
var environmentName = builder.Environment.EnvironmentName ?? "Development";

// Use the correct Loki OTLP endpoint
var lokiEndpoint = configuration["OTEL_EXPORTER_OTLP_LOGS_ENDPOINT"] ?? "http://loki:3100";

var resourceBuilder = OpenTelemetryExtensions.CreateServiceResourceBuilder(serviceName, environmentName);

builder.AddOpenTelemetry(lokiEndpoint, resourceBuilder);

builder.Services
    .AddDatabaseConfig<ApplicationDbContext>(builder.Configuration)
    .AddRepository<Account, AccountRepository, IAccountRepository, ApplicationDbContext>()
    .AddRepositoriesConfig<IJwtTokenService, JwtTokenService>()
    .AddRepositoriesConfig<IPasswordService, PasswordService>()
    .AddRepositoriesConfig<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>()
    .AddAplicationConfig(typeof(CreateAccountCommand).Assembly, typeof(CreateAccountCommandValidator).Assembly)
    .AddPresentation<IdentityExceptionMapper>(builder.Configuration, otelEndpoint, serviceName, environmentName);

var app = builder.Build();

app.MigrateDatabaseConfig<ApplicationDbContext>();

app.UseGlobalExceptionHandler<Program>()
    .UseRequestDurationLogging<Program>()
    .UseStandardMiddleware()
    .MapStandardEndpoints();

app.MapApiDocumentation();
app.MapEndpoints(typeof(UserEndpointGroup).Assembly);

app.Logger.LogInformation("🚀 {ServiceName} starting up in {Environment} environment", serviceName, environmentName);

app.Run();