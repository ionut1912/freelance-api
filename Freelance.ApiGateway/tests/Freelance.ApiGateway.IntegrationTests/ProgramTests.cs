using System.Net;

namespace Freelance.ApiGateway.IntegrationTests;

public sealed class ProgramTests(GatewayFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task HealthCheck_IsHealthy() =>
        (await _client.GetAsync("/healthz", _ct)).StatusCode.Should().Be(HttpStatusCode.OK);

    [Fact]
    public async Task ScalarReference_IsServedByTheGatewayNotOcelot()
    {
        var response = await _client.GetAsync("/scalar/", _ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync(_ct)).Should().Contain("Freelance API Gateway");
    }

    [Theory]
    [InlineData("http://localhost:3000", true)]
    [InlineData("https://evil.example", false)]
    public async Task CorsPreflight_AllowsOnlyConfiguredOrigins(string origin, bool allowed)
    {
        using var request = new HttpRequestMessage(HttpMethod.Options, "/project/42");
        request.Headers.Add("Origin", origin);
        request.Headers.Add("Access-Control-Request-Method", "GET");
        request.Headers.Add("Access-Control-Request-Headers", "authorization");

        var response = await _client.SendAsync(request, _ct);

        response.Headers.TryGetValues("Access-Control-Allow-Origin", out var values).Should().Be(allowed);
        if (allowed) values.Should().ContainSingle().Which.Should().Be(origin);
    }
}
