using System.Net;
using System.Net.Http.Json;
using Freelance.Face.Api.Endpoints;
using Microsoft.AspNetCore.Hosting;

namespace Freelance.Face.IntegrationTests.Endpoints;

public sealed class CameraEndpointGroupTests(FaceApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task CreateCaptureSession_ReturnsSessionAndDeepLink()
    {
        var response = await factory.CreateClient().PostAsync("/camera/capture/sessions", null, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var session = await response.Content.ReadFromJsonAsync<CaptureSessionResponse>(_ct);
        session!.SessionId.Should().MatchRegex("^[0-9a-f]{32}$");
        session.DeepLink.Should().Be($"https://frontend.test/remote-capture?session={session.SessionId}");
    }

    [Fact]
    public async Task CreateCaptureSession_EachCallCreatesANewSession()
    {
        var client = factory.CreateClient();

        var first = await (await client.PostAsync("/camera/capture/sessions", null, _ct)).Content.ReadFromJsonAsync<CaptureSessionResponse>(_ct);
        var second = await (await client.PostAsync("/camera/capture/sessions", null, _ct)).Content.ReadFromJsonAsync<CaptureSessionResponse>(_ct);

        first!.SessionId.Should().NotBe(second!.SessionId);
    }

    [Fact]
    public async Task CreateCaptureSession_WithoutFrontendUrl_ReturnsProblem()
    {
        using var misconfigured = factory.WithWebHostBuilder(b => b.UseSetting("FRONTEND_BASE_URL", ""));

        var response = await misconfigured.CreateClient().PostAsync("/camera/capture/sessions", null, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        (await response.Content.ReadAsStringAsync(_ct)).Should().Contain("FRONTEND_BASE_URL is not set.");
    }
}
