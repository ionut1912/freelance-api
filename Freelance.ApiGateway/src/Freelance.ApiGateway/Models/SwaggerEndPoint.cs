namespace Freelance.ApiGateway.Models;

public sealed class SwaggerEndPoint
{
    public string Key { get; set; } = string.Empty;
    public List<SwaggerConfig> Config { get; set; } = [];
}
