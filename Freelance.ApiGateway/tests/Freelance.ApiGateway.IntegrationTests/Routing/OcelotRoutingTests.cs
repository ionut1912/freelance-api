using System.Net;
using System.Net.Http.Json;

namespace Freelance.ApiGateway.IntegrationTests.Routing;

public sealed class OcelotRoutingTests(GatewayFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Theory]
    [InlineData("POST", "/auth/register", "http://freelance_identity:8080/auth/register")]
    [InlineData("POST", "/auth/login", "http://freelance_identity:8080/auth/login")]
    public async Task AnonymousRoutes_AreForwardedWithoutToken(string method, string path, string downstream)
    {
        var response = await factory.CreateClient().SendAsync(new HttpRequestMessage(new HttpMethod(method), path), _ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<DownstreamEcho>(_ct)).Should()
            .BeEquivalentTo(new DownstreamEcho(method, downstream, HasAuthorization: false));
    }

    [Theory]
    [InlineData("GET", "/auth/current", "http://freelance_identity:8080/auth/current")]
    [InlineData("POST", "/camera/capture/sessions", "http://freelance_face:8080/camera/capture/sessions")]
    [InlineData("GET", "/client-profiles/current", "http://freelance_user_profiles:8080/client-profiles/current")]
    [InlineData("PUT", "/freelancer-profiles/verify", "http://freelance_user_profiles:8080/freelancer-profiles/verify")]
    [InlineData("GET", "/project/42", "http://freelance_project_management:8080/project/42")]
    [InlineData("DELETE", "/projectTask/42", "http://freelance_project_management:8080/projectTask/42")]
    [InlineData("POST", "/timeLog/x", "http://freelance_project_management:8080/timeLog/x")]
    public async Task ProtectedRoutes_WithToken_AreForwardedWithAuthorization(string method, string path, string downstream)
    {
        var response = await factory.CreateAuthenticatedClient().SendAsync(new HttpRequestMessage(new HttpMethod(method), path), _ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<DownstreamEcho>(_ct)).Should()
            .BeEquivalentTo(new DownstreamEcho(method, downstream, HasAuthorization: true));
    }

    [Theory]
    [InlineData("/auth/current")]
    [InlineData("/client-profiles/current")]
    [InlineData("/project/42")]
    public async Task ProtectedRoutes_WithoutToken_AreRejectedByTheGateway(string path) =>
        (await factory.CreateClient().GetAsync(path, _ct)).StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    [Fact]
    public async Task ProtectedRoutes_WithForgedToken_AreRejected()
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new("Bearer", GatewayFactory.CreateToken() + "tampered");

        (await client.GetAsync("/project/42", _ct)).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UnknownRoute_ReturnsNotFound() =>
        (await factory.CreateClient().GetAsync("/unknown/route", _ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
}
