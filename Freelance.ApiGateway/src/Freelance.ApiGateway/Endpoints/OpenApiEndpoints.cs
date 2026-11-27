using Freelance.ApiGateway.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi;

namespace Freelance.ApiGateway.Endpoints;

public static class OpenApiEndpoints
{
    public const string DocumentPath = "/openapi/v1.json";
    public const string CachePolicy = "openapi";

    extension(IEndpointRouteBuilder app)
    {
        /// <summary>Serves the aggregated OpenAPI 3.1 document of all downstream services.</summary>
        public IEndpointRouteBuilder MapAggregatedOpenApi()
        {
            app.MapGet(DocumentPath, GetOpenApiAsync)
                .WithName("GetAggregatedOpenApi")
                .ExcludeFromDescription()
                .CacheOutput(CachePolicy);

            return app;
        }
    }

    private static async Task<ContentHttpResult> GetOpenApiAsync(OpenApiAggregatorService aggregator,
        CancellationToken cancellationToken)
    {
        var document = await aggregator.AggregateOpenApiDocsAsync(cancellationToken);
        var json = await document.SerializeAsJsonAsync(OpenApiSpecVersion.OpenApi3_1, cancellationToken);
        return TypedResults.Text(json, "application/json");
    }
}
