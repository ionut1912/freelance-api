namespace Freelance.ApiGateway.Models;

/// <summary>The parts of the gateway configuration the aggregated OpenAPI document is built from.</summary>
public sealed class OpenApiAggregationOptions
{
    public List<RouteConfig> Routes { get; set; } = [];
    public List<SwaggerEndPoint> SwaggerEndPoints { get; set; } = [];
    public GlobalConfigurationOptions GlobalConfiguration { get; set; } = new();
}

/// <summary>Ocelot's GlobalConfiguration section (only the value used here).</summary>
public sealed class GlobalConfigurationOptions
{
    public string? BaseUrl { get; set; }
}
