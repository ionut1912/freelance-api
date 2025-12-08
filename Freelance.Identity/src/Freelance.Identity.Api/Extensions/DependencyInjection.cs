using Freelance.Identity.Api.Handlers;
using Microsoft.AspNetCore.Diagnostics;
using OpenTelemetry.Resources;
using Shared.Api.Extensions;

namespace Freelance.Identity.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        IConfiguration configuration,
        string otelEndpoint,
        ResourceBuilder resourceBuilder)
    {
        // Authentication & Authorization
        services
            .AddJwtAuthentication(configuration)
            .AddRoleBasedAuthorization();

        // Observability
        services.AddOpenTelemetryObservability(otelEndpoint, "Freelance-Identity", resourceBuilder);

        // API Documentation
        services.AddOpenApiWithJwtAuth("Freelance Identity API");

        // Exception Handling
        services.AddSingleton<IExceptionHandler, ExceptionHandler>();

        // Health Checks
        services.AddHealthChecks();

        // API Infrastructure
        services.AddEndpointsApiExplorer();
        services.AddControllers();

        return services;
    }
}
