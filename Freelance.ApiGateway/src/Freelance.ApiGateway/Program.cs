using Freelance.ApiGateway.Endpoints;
using Freelance.ApiGateway.Services;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Scalar.AspNetCore;
using Shared.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var serviceName = configuration["OTEL_SERVICE_NAME"] ?? "Freelance-Api-Gateway";
var otelEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://tempo:4317";
var lokiEndpoint = configuration["OTEL_EXPORTER_OTLP_LOGS_ENDPOINT"] ?? "http://loki:3100";

builder.AddOpenTelemetry(lokiEndpoint,
    OpenTelemetryExtensions.CreateServiceResourceBuilder(serviceName, builder.Environment.EnvironmentName));

const string FrontendCorsPolicy = "frontend";
string[] allowedOrigins =
[
    .. configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [],
    .. configuration["FRONTEND_BASE_URL"] is { Length: > 0 } frontendUrl ? [frontendUrl] : Array.Empty<string>()
];

builder.Services
    .AddOpenTelemetryObservability(otelEndpoint, serviceName)
    // Default scheme "Bearer" is the AuthenticationProviderKey used by the Ocelot routes.
    .AddJwtAuthentication(configuration)
    .AddAuthorization()
    .AddCors(options => options.AddPolicy(FrontendCorsPolicy, policy => policy
        .WithOrigins(allowedOrigins)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()))
    .AddOutputCache(options => options.AddPolicy(OpenApiEndpoints.CachePolicy,
        policy => policy.Expire(TimeSpan.FromMinutes(1))))
    .AddOcelot(configuration);

builder.Services.AddHealthChecks();
builder.Services.AddHttpClient<OpenApiAggregatorService>(client => client.Timeout = TimeSpan.FromSeconds(10));

var app = builder.Build();

app.UseRouting();
app.UseCors(FrontendCorsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.UseOutputCache();

app.MapAggregatedOpenApi();
app.MapHealthChecks("/healthz");
app.MapScalarApiReference(options => options
    .WithTitle("Freelance API Gateway")
    .WithTheme(ScalarTheme.Purple)
    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
    .WithOpenApiRoutePattern(OpenApiEndpoints.DocumentPath));

// Gateway endpoints (OpenAPI, Scalar, health) run here; any other request falls through to Ocelot,
// which is terminal middleware.
app.UseEndpoints(static _ => { });
await app.UseOcelot();

await app.RunAsync();
