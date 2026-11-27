using Freelance.ApiGateway.Models;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Reader;

namespace Freelance.ApiGateway.Services;

/// <summary>
/// Downloads the OpenAPI documents of every downstream service (in parallel) and merges them into a single
/// document whose paths are expressed as gateway (upstream) paths. Uses the Microsoft.OpenApi 2.x object model.
/// </summary>
public sealed class OpenApiAggregatorService(
    HttpClient httpClient,
    IOptionsMonitor<OpenApiAggregationOptions> options,
    ILogger<OpenApiAggregatorService> logger)
{
    private const string BearerSchemeName = "Bearer";
    private const string DefaultBaseUrl = "http://localhost:9000";

    public async Task<OpenApiDocument> AggregateOpenApiDocsAsync(CancellationToken cancellationToken = default)
    {
        // Read once per call, so a configuration reload applies to the next document.
        var settings = options.CurrentValue;
        var aggregated = CreateBaseDocument(settings.GlobalConfiguration.BaseUrl);
        var routes = settings.Routes;
        var endpoints = settings.SwaggerEndPoints;

        var downloads = endpoints
            .SelectMany(endpoint => endpoint.Config.Select(config => (endpoint.Key, Config: config)))
            .Select(async item => (item.Key, item.Config,
                Document: await FetchOpenApiDocAsync(item.Config, cancellationToken)));

        // Merge sequentially, in configuration order, so the result is deterministic.
        foreach (var (key, config, document) in await Task.WhenAll(downloads))
        {
            if (document is null) continue;

            logger.LogInformation("Merging {ServiceName} ({PathCount} paths)", config.Name, document.Paths?.Count ?? 0);
            MergePaths(aggregated, document, key, routes);
            MergeComponents(aggregated, document);
            MergeTags(aggregated, document);
        }

        logger.LogInformation("OpenAPI aggregation complete: {PathCount} paths", aggregated.Paths.Count);
        return aggregated;
    }

    private static OpenApiDocument CreateBaseDocument(string? baseUrl)
    {
        var document = new OpenApiDocument
        {
            Info = new OpenApiInfo
            {
                Title = "Freelance API Gateway",
                Version = "v1",
                Description = "Combined API documentation from all downstream services"
            },
            Servers = [new OpenApiServer { Url = baseUrl ?? DefaultBaseUrl }],
            Paths = [],
            Components = new OpenApiComponents
            {
                Schemas = new Dictionary<string, IOpenApiSchema>(),
                SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                {
                    [BearerSchemeName] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "Enter your JWT token"
                    }
                }
            },
            Tags = new HashSet<OpenApiTag>()
        };

        document.Security =
        [
            new OpenApiSecurityRequirement { [new OpenApiSecuritySchemeReference(BearerSchemeName, document)] = [] }
        ];

        return document;
    }

    private async Task<OpenApiDocument?> FetchOpenApiDocAsync(SwaggerConfig config, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await httpClient.GetAsync(config.Url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to fetch OpenAPI doc of {ServiceName}: {StatusCode} ({Url})", config.Name,
                    response.StatusCode, config.Url);
                return null;
            }

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var result = await OpenApiDocument.LoadAsync(stream, OpenApiConstants.Json, new OpenApiReaderSettings(),
                cancellationToken);

            if (result.Diagnostic?.Errors is { Count: > 0 } errors)
            {
                logger.LogWarning("OpenAPI doc of {ServiceName} has errors: {Errors}", config.Name,
                    string.Join(", ", errors.Select(e => e.Message)));
            }

            return result.Document;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or OpenApiException)
        {
            // A single unavailable service must not break the whole gateway documentation.
            logger.LogError(ex, "Error fetching OpenAPI doc of {ServiceName} at {Url}", config.Name, config.Url);
            return null;
        }
    }

    private void MergePaths(OpenApiDocument target, OpenApiDocument source, string swaggerKey,
        List<RouteConfig> routes)
    {
        if (source.Paths is null) return;

        foreach (var (path, item) in source.Paths)
        {
            var gatewayPath = OpenApiPathTranslator.ToUpstreamPath(path, swaggerKey, routes);
            if (!target.Paths.TryAdd(gatewayPath, item))
                logger.LogWarning("Duplicate path {Path} ignored (from {SwaggerKey})", gatewayPath, swaggerKey);
        }
    }

    private static void MergeComponents(OpenApiDocument target, OpenApiDocument source)
    {
        // NOTE: schema names are expected to be unique across services ($ref values are not rewritten).
        foreach (var (name, schema) in source.Components?.Schemas ?? new Dictionary<string, IOpenApiSchema>())
            target.Components!.Schemas!.TryAdd(name, schema);

        foreach (var (name, scheme) in source.Components?.SecuritySchemes ?? new Dictionary<string, IOpenApiSecurityScheme>())
            target.Components!.SecuritySchemes!.TryAdd(name, scheme);
    }

    private static void MergeTags(OpenApiDocument target, OpenApiDocument source)
    {
        foreach (var tag in source.Tags ?? new HashSet<OpenApiTag>())
            target.Tags!.Add(tag);
    }
}
