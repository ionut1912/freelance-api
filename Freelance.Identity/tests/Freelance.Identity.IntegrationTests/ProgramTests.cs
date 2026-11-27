using System.Net;

namespace Freelance.Identity.IntegrationTests;

public sealed class ProgramTests(IdentityApiFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Theory]
    [InlineData("/healthz")]
    [InlineData("/ready")]
    [InlineData("/live")]
    public async Task HealthEndpoints_AreHealthy(string url) =>
        (await _client.GetAsync(url, _ct)).StatusCode.Should().Be(HttpStatusCode.OK);

    [Fact]
    public async Task OpenApiDocument_DescribesAuthEndpointsWithBearerSecurity()
    {
        var body = await _client.GetStringAsync("/openapi/v1.json", _ct);

        body.Should().Contain("\"/auth/register\"").And.Contain("\"/auth/login\"").And.Contain("\"Bearer\"");
    }

    [Fact]
    public async Task ScalarReference_IsServed() =>
        (await _client.GetAsync("/scalar/", _ct)).StatusCode.Should().Be(HttpStatusCode.OK);
}
