using Freelance.ApiGateway.Models;

namespace Freelance.ApiGateway.Services;

/// <summary>Maps a downstream OpenAPI path to the corresponding gateway (Ocelot upstream) path.</summary>
public static class OpenApiPathTranslator
{
    public static string ToUpstreamPath(string downstreamPath, string swaggerKey, IEnumerable<RouteConfig> routes)
    {
        var route = routes.FirstOrDefault(r =>
            r.SwaggerKey == swaggerKey &&
            r.DownstreamPathTemplate is not null &&
            r.UpstreamPathTemplate is not null &&
            Matches(downstreamPath, r.DownstreamPathTemplate));

        if (route is not null)
            return Translate(downstreamPath, route.DownstreamPathTemplate!, route.UpstreamPathTemplate!);

        // Legacy convention: strip an "/api" prefix when no route matches.
        return downstreamPath.StartsWith("/api/", StringComparison.Ordinal) ? downstreamPath[4..] : downstreamPath;
    }

    private static bool IsPlaceholder(string segment) => segment is ['{', .., '}'];

    private static bool Matches(string path, string template)
    {
        var pathParts = path.Split('/');
        var templateParts = template.Split('/');

        // "{everything}" is Ocelot's catch-all: it matches the rest of the path.
        if (templateParts is [.., "{everything}"])
        {
            var prefix = templateParts[..^1];
            return pathParts.Length >= templateParts.Length &&
                   prefix.Select((part, i) => IsPlaceholder(part) || part == pathParts[i]).All(match => match);
        }

        return pathParts.Length == templateParts.Length &&
               templateParts.Select((part, i) => IsPlaceholder(part) || part == pathParts[i]).All(match => match);
    }

    private static string Translate(string path, string downstreamTemplate, string upstreamTemplate)
    {
        var pathParts = path.Split('/');
        var downstreamParts = downstreamTemplate.Split('/');

        var values = new Dictionary<string, string>(StringComparer.Ordinal);
        for (var i = 0; i < downstreamParts.Length && i < pathParts.Length; i++)
        {
            if (!IsPlaceholder(downstreamParts[i])) continue;

            values[downstreamParts[i]] = downstreamParts[i] == "{everything}"
                ? string.Join('/', pathParts[i..])
                : pathParts[i];
        }

        var result = upstreamTemplate
            .Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Select(part => IsPlaceholder(part) && values.TryGetValue(part, out var value) ? value : part)
            .Where(part => part.Length > 0);

        return "/" + string.Join('/', result);
    }
}
