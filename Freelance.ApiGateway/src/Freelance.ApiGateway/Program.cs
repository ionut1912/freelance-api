using Freelance.ApiGateway.Endpoints;
using Freelance.ApiGateway.Extensions;
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

builder.Services
    .AddOpenTelemetryObservability(otelEndpoint, serviceName)
    // Default scheme "Bearer" is the AuthenticationProviderKey used by the Ocelot routes.
    .AddJwtAuthentication(configuration)
    .AddAuthorization()
    .AddFrontendCors(configuration)
    .AddAggregatedOpenApi(configuration)
    .AddOcelot(configuration);

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseRouting();
app.UseCors(FrontendCorsExtensions.PolicyName);
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
