using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Scalar.AspNetCore;
using Shared.Api.Extensions;
using System.Text;
using ServiceCollectionExtensions = Shared.Api.Extensions.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://+:8080");
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var otelEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://alloy:4317";
var serviceName = builder.Configuration["OTEL_SERVICE_NAME"] ?? "Freelance-Gateway";
var environmentName = builder.Environment.EnvironmentName;

builder.AddOpenTelemetry(otelEndpoint, serviceName, environmentName);

var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing"));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

var resourceBuilder = ServiceCollectionExtensions.CreateServiceResourceBuilder(serviceName, environmentName);
builder.Services.AddOpenTelemetryObservability(otelEndpoint, serviceName, resourceBuilder);

builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddOpenApiWithJwtAuth("Freelance API Gateway");
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseRequestDurationLogging<Program>();

app.UseStandardMiddleware();

app.MapApiDocumentation(options =>
{
    options
        .WithTitle("Freelance API Gateway")
        .WithTheme(ScalarTheme.Default)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.MapStandardEndpoints();

await app.UseOcelot();

app.Run();