using Freelance.Identity.Api.Endpoints;
using Freelance.Identity.Api.Mappers;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Validators;
using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Infrastructure.Persistance;
using Freelance.Identity.Infrastructure.Persistance.Repositories;
using Shared.Api.Extensions;
using Shared.Domain.Interfaces;
using Shared.Infra.Services;

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

app.MapUsersEndpoints();

app.Run();