using Freelance.ApiGateway.Models;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Writers;

namespace ApiGateway.Services;

public class OpenApiAggregatorService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OpenApiAggregatorService> _logger;

    public OpenApiAggregatorService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<OpenApiAggregatorService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<OpenApiDocument> AggregateOpenApiDocsAsync()
    {
        var aggregatedDoc = CreateBaseDocument();
        var swaggerEndPoints = GetSwaggerEndPoints();

        _logger.LogInformation("Starting to aggregate OpenAPI docs from {Count} swagger endpoints", swaggerEndPoints.Count);

        foreach (var endPoint in swaggerEndPoints)
        {
            await ProcessEndPointAsync(endPoint, aggregatedDoc);
        }

        _logger.LogInformation("Aggregation complete. Total paths: {PathCount}", aggregatedDoc.Paths.Count);
        return aggregatedDoc;
    }

    private OpenApiDocument CreateBaseDocument()
    {
        return new OpenApiDocument
        {
            Info = new OpenApiInfo
            {
                Title = "Freelance API Gateway",
                Version = "v1",
                Description = "Combined API documentation from all downstream services"
            },
            Servers = new List<OpenApiServer>
            {
                new OpenApiServer { Url = _configuration["GlobalConfiguration:BaseUrl"] ?? "http://localhost:9000" }
            },
            Paths = new OpenApiPaths(),
            Components = new OpenApiComponents
            {
                SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
                {
                    ["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "Enter your JWT token"
                    }
                },
                Schemas = new Dictionary<string, OpenApiSchema>()
            }
        };
    }

    private List<SwaggerEndPoint> GetSwaggerEndPoints()
    {
        return _configuration.GetSection("SwaggerEndPoints")
            .Get<List<SwaggerEndPoint>>() ?? new List<SwaggerEndPoint>();
    }

    private async Task ProcessEndPointAsync(SwaggerEndPoint endPoint, OpenApiDocument aggregatedDoc)
    {
        if (endPoint.Config == null || !endPoint.Config.Any())
            return;

        foreach (var config in endPoint.Config)
        {
            try
            {
                _logger.LogInformation("Fetching OpenAPI doc from {ServiceName} at {Url}", config.Name, config.Url);
                var openApiDoc = await FetchOpenApiDocAsync(config.Url);

                if (openApiDoc != null)
                {
                    _logger.LogInformation("Successfully fetched {ServiceName}. Paths: {PathCount}",
                        config.Name, openApiDoc.Paths?.Count ?? 0);

                    MergeOpenApiDoc(aggregatedDoc, openApiDoc, endPoint.Key);
                }
                else
                {
                    _logger.LogWarning("Received null document from {ServiceName}", config.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch OpenAPI doc from {ServiceName} at {Url}",
                    config.Name, config.Url);
            }
        }
    }

    private async Task<OpenApiDocument?> FetchOpenApiDocAsync(string url)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            _logger.LogDebug("Sending request to {Url}", url);
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch OpenAPI doc. Status: {StatusCode}, Url: {Url}",
                    response.StatusCode, url);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogDebug("Received content length: {Length} bytes", content.Length);

            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogWarning("Received empty response from {Url}", url);
                return null;
            }

            return ParseOpenApiDocument(content, url);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout fetching OpenAPI doc from {Url}", url);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching OpenAPI doc from {Url}", url);
            return null;
        }
    }

    private OpenApiDocument? ParseOpenApiDocument(string content, string url)
    {
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
        var reader = new OpenApiStreamReader();
        var document = reader.Read(stream, out var diagnostic);

        if (diagnostic?.Errors?.Count > 0)
        {
            _logger.LogWarning("OpenAPI document has errors: {Errors}",
                string.Join(", ", diagnostic.Errors.Select(e => e.Message)));
        }

        if (document == null)
        {
            _logger.LogWarning("Failed to parse OpenAPI document from {Url}", url);
            return null;
        }

        return document;
    }

    private void MergeOpenApiDoc(OpenApiDocument target, OpenApiDocument source, string swaggerKey)
    {
        MergePaths(target, source, swaggerKey);
        MergeSchemas(target, source);
        MergeSecuritySchemes(target, source);
    }

    private  void MergePaths(OpenApiDocument target, OpenApiDocument source, string swaggerKey)
    {
        if (source.Paths == null) return;

        var routes = _configuration.GetSection("Routes").Get<List<RouteConfig>>() ?? new List<RouteConfig>();

        foreach (var path in source.Paths)
        {
            var newPath = TransformPath(path.Key, swaggerKey, routes);

            if (!target.Paths.ContainsKey(newPath))
            {
                target.Paths.Add(newPath, path.Value);
                _logger.LogDebug("Added path: {Path} (original: {OriginalPath})", newPath, path.Key);
            }
            else
            {
                _logger.LogWarning("Duplicate path found: {Path}", newPath);
            }
        }
    }

    private static string TransformPath(string originalPath, string swaggerKey, List<RouteConfig> routes)
    {
        if (!originalPath.StartsWith("/api/"))
        {
            return originalPath;
        }

        var pathAfterApi = originalPath.Substring(4);

        var matchingRoute = routes.FirstOrDefault(r =>
            r.SwaggerKey == swaggerKey &&
            r.DownstreamPathTemplate != null &&
            PathMatchesTemplate(originalPath, r.DownstreamPathTemplate));

        if (matchingRoute?.UpstreamPathTemplate != null&& matchingRoute?.DownstreamPathTemplate!=null)
        {
            return TransformWithTemplate(originalPath, matchingRoute.DownstreamPathTemplate, matchingRoute.UpstreamPathTemplate);
        }

        return pathAfterApi;
    }

    private static bool PathMatchesTemplate(string path, string template)
    {
        var pathParts = path.Split('/');
        var templateParts = template.Split('/');

        if (pathParts.Length != templateParts.Length)
        {
            return false;
        }

        for (int i = 0; i < pathParts.Length; i++)
        {
            if (templateParts[i].StartsWith("{") && templateParts[i].EndsWith("}"))
            {
                continue;
            }

            if (pathParts[i] != templateParts[i])
            {
                return false;
            }
        }

        return true;
    }

    private static string TransformWithTemplate(string originalPath, string downstreamTemplate, string upstreamTemplate)
    {
        var pathParts = originalPath.Split('/');
        var downstreamParts = downstreamTemplate.Split('/');
        var upstreamParts = upstreamTemplate.Split('/');

        var parameters = new Dictionary<string, string>();

        for (int i = 0; i < downstreamParts.Length; i++)
        {
            if (downstreamParts[i].StartsWith("{") && downstreamParts[i].EndsWith("}"))
            {
                var paramName = downstreamParts[i];
                parameters[paramName] = pathParts[i];
            }
        }

        var result = new List<string>();
        foreach (var part in upstreamParts)
        {
            if (part.StartsWith("{") && part.EndsWith("}"))
            {
                if (parameters.TryGetValue(part, out var value))
                {
                    result.Add(value);
                }
                else
                {
                    result.Add(part);
                }
            }
            else if (!string.IsNullOrEmpty(part))
            {
                result.Add(part);
            }
        }

        return "/" + string.Join("/", result);
    }

    private static void MergeSchemas(OpenApiDocument target, OpenApiDocument source)
    {
        if (source.Components?.Schemas == null) return;

        foreach (var schema in source.Components.Schemas)
        {
            var schemaKey = schema.Key;
            var counter = 1;

            while (target.Components.Schemas.ContainsKey(schemaKey))
            {
                schemaKey = $"{schema.Key}{counter}";
                counter++;
            }

            target.Components.Schemas.Add(schemaKey, schema.Value);
        }
    }

    private static void MergeSecuritySchemes(OpenApiDocument target, OpenApiDocument source)
    {
        if (source.Components?.SecuritySchemes == null) return;

        foreach (var securityScheme in source.Components.SecuritySchemes)
        {
            if (!target.Components.SecuritySchemes.ContainsKey(securityScheme.Key))
            {
                target.Components.SecuritySchemes.Add(securityScheme.Key, securityScheme.Value);
            }
        }
    }
}