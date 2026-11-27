using System.Net;

namespace Freelance.UserProfiles.IntegrationTests;

public sealed class ProgramTests(UserProfilesApiFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Theory]
    [InlineData("/healthz")]
    [InlineData("/ready")]
    public async Task HealthEndpoints_AreHealthy(string url) =>
        (await _client.GetAsync(url, _ct)).StatusCode.Should().Be(HttpStatusCode.OK);

    [Fact]
    public async Task OpenApiDocument_DescribesBothProfileGroups() =>
        (await _client.GetStringAsync("/openapi/v1.json", _ct))
        .Should().Contain("\"/client-profiles\"").And.Contain("\"/freelancer-profiles/details/{id}\"");
}
