using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Shared.Api.Extensions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://+:8080");
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

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

var configuration = builder.Configuration;
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://tempo:4317";
var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-Api-Gateway";
var environmentName = builder.Environment.EnvironmentName ?? "Development";
var lokiEndpoint = configuration["OTEL_EXPORTER_OTLP_LOGS_ENDPOINT"] ?? "http://loki:3100";
var resourceBuilder = OpenTelemetryExtensions.CreateServiceResourceBuilder(serviceName, environmentName);
builder.AddOpenTelemetry(lokiEndpoint, resourceBuilder);
builder.Services.AddOpenTelemetryObservability(otelEndpoint,serviceName);

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerForOcelotUI();

await app.UseOcelot();

app.Run();