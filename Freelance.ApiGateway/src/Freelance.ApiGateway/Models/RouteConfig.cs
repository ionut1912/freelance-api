namespace Freelance.ApiGateway.Models;

/// <summary>Subset of an Ocelot route used to translate downstream OpenAPI paths to gateway paths.</summary>
public sealed class RouteConfig
{
    public string? UpstreamPathTemplate { get; set; }
    public string? DownstreamPathTemplate { get; set; }
    public string? SwaggerKey { get; set; }
}
