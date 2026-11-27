using System.Net;

namespace Freelance.ProjectManagement.IntegrationTests;

public sealed class ProgramTests(ProjectManagementApiFactory factory)
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
    public async Task OpenApiDocument_DescribesAllGroups() =>
        (await _client.GetStringAsync("/openapi/v1.json", _ct))
        .Should().Contain("\"/project\"").And.Contain("\"/projectTask/{id}\"").And.Contain("\"/timeLog\"");
}
