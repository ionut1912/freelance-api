namespace Freelance.ApiGateway.Models;

public class RouteConfig
{
    public string? UpstreamPathTemplate { get; set; }
    public string? DownstreamPathTemplate { get; set; }
    public string? SwaggerKey { get; set; }
}
