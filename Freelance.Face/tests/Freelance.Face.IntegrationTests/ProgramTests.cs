using System.Net;

namespace Freelance.Face.IntegrationTests;

public sealed class ProgramTests(FaceApiFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Theory]
    [InlineData("/healthz")]
    [InlineData("/live")]
    public async Task HealthEndpoints_AreHealthy(string url) =>
        (await _client.GetAsync(url, _ct)).StatusCode.Should().Be(HttpStatusCode.OK);

    [Fact]
    public async Task OpenApiDocument_DescribesTheCameraEndpoint() =>
        (await _client.GetStringAsync("/openapi/v1.json", _ct)).Should().Contain("\"/camera/capture/sessions\"");

    [Fact]
    public async Task CaptureHub_AcceptsSignalRNegotiation()
    {
        var response = await _client.PostAsync("/hubs/capture/negotiate?negotiateVersion=1", null, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync(_ct)).Should().Contain("connectionToken");
    }

    [Fact]
    public async Task UnknownRoute_ReturnsNotFound() =>
        (await _client.GetAsync("/does-not-exist", _ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
}
