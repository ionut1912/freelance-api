using ApiGateway.Services;

namespace Freelance.ApiGateway.Models;

public class SwaggerEndPoint
{
    public string Key { get; set; } = string.Empty;
    public List<SwaggerConfig> Config { get; set; } = new();
}
