namespace Freelance.ApiGateway.Endponts;

public static class Enpoints
{
    public static IEndpointRouteBuilder MapOpenApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapOpenApi();
        return app;
    }
}
