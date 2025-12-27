using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;

namespace ApiGateway.Services;

[ApiController]
[Route("openapi")]
public class OpenApiController : ControllerBase
{
    private readonly OpenApiAggregatorService _aggregator;
    private readonly ILogger<OpenApiController> _logger;

    public OpenApiController(OpenApiAggregatorService aggregator, ILogger<OpenApiController> logger)
    {
        _aggregator = aggregator;
        _logger = logger;
    }

    [HttpGet("v1.json")]
    public async Task<IActionResult> GetAggregatedOpenApi()
    {
        try
        {
            _logger.LogInformation("Generating aggregated OpenAPI document");
            var doc = await _aggregator.AggregateOpenApiDocsAsync();

            using var outputString = new StringWriter();
            var writer = new OpenApiJsonWriter(outputString);
            doc.SerializeAsV3(writer);

            var json = outputString.ToString();
            _logger.LogInformation("Generated OpenAPI document with {Length} characters", json.Length);

            return Content(json, "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating aggregated OpenAPI document");
            return StatusCode(500, new { error = "Failed to generate OpenAPI document", details = ex.Message });
        }
    }
}