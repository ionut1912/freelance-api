using Freelance.ApiGateway.Models;
using Freelance.ApiGateway.Services;

namespace Freelance.ApiGateway.UnitTests.Services;

public sealed class OpenApiPathTranslatorTests
{
    private static readonly List<RouteConfig> Routes =
    [
        new() { SwaggerKey = "auth", DownstreamPathTemplate = "/auth/login", UpstreamPathTemplate = "/auth/login" },
        new() { SwaggerKey = "auth", DownstreamPathTemplate = "/auth/{everything}", UpstreamPathTemplate = "/identity/{everything}" },
        new() { SwaggerKey = "projects", DownstreamPathTemplate = "/project/{id}/tasks", UpstreamPathTemplate = "/p/{id}/t" },
        new() { SwaggerKey = "broken", DownstreamPathTemplate = null, UpstreamPathTemplate = "/x" }
    ];

    [Theory]
    [InlineData("/auth/login", "auth", "/auth/login")]
    [InlineData("/auth/current", "auth", "/identity/current")]
    [InlineData("/auth/a/b", "auth", "/identity/a/b")]
    [InlineData("/project/{id}/tasks", "projects", "/p/{id}/t")]
    public void ToUpstreamPath_MatchingRoute_TranslatesTemplate(string downstream, string key, string expected) =>
        OpenApiPathTranslator.ToUpstreamPath(downstream, key, Routes).Should().Be(expected);

    [Theory]
    [InlineData("/api/things", "other", "/things")]
    [InlineData("/things", "other", "/things")]
    [InlineData("/auth/login", "projects", "/auth/login")]
    [InlineData("/project/1", "projects", "/project/1")]
    public void ToUpstreamPath_NoMatchingRoute_FallsBack(string downstream, string key, string expected) =>
        OpenApiPathTranslator.ToUpstreamPath(downstream, key, Routes).Should().Be(expected);
}
