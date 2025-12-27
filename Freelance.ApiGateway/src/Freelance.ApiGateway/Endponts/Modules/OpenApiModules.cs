using ApiGateway.Services;
using Microsoft.OpenApi.Writers;

namespace Freelance.ApiGateway.Endponts.Modules;

public static class OpenApiModules
{
    public static IEndpointRouteBuilder AddOpenApiEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapGet("/openapi/v1.json", GetOpenApi);

        return app;
    }

    private static async Task<IResult> GetOpenApi(
        ILoggerFactory loggerFactory,
        OpenApiAggregatorService aggregator)
    {
        var logger = loggerFactory.CreateLogger("OpenApi");

        try
        {
            logger.LogInformation("Generating aggregated OpenAPI document");

            var doc = await aggregator.AggregateOpenApiDocsAsync();

            using var output = new StringWriter();
            var writer = new OpenApiJsonWriter(output);
            doc.SerializeAsV3(writer);

            var json = output.ToString();

            logger.LogInformation(
                "Generated OpenAPI document with {Length} characters",
                json.Length);

            return Results.Text(json, "application/json");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating aggregated OpenAPI document");

            return Results.Problem(
                title: "Failed to generate OpenAPI document",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
