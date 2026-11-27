using System.Net;
using Freelance.ApiGateway.Endpoints;

namespace Freelance.ApiGateway.IntegrationTests.Endpoints;

public sealed class OpenApiEndpointsTests(GatewayFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task AggregatedDocument_ContainsReachableServicesAsOpenApi31()
    {
        var response = await factory.CreateClient().GetAsync(OpenApiEndpoints.DocumentPath, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
        var body = await response.Content.ReadAsStringAsync(_ct);
        body.Should().Contain("\"openapi\": \"3.1").And.Contain("\"/auth/login\"").And.Contain("\"/project\"")
            .And.Contain("Freelance API Gateway").And.Contain("\"Bearer\"");
    }

    [Fact]
    public async Task AggregatedDocument_IsOutputCached()
    {
        var client = factory.CreateClient();
        var first = await client.GetStringAsync(OpenApiEndpoints.DocumentPath, _ct); // warms the cache
        var downstreamCalls = factory.OpenApiHandler.Invocations.Count;

        var second = await client.GetStringAsync(OpenApiEndpoints.DocumentPath, _ct);

        second.Should().Be(first);
        factory.OpenApiHandler.Invocations.Count.Should().Be(downstreamCalls, "a cached document is served without calling the services again");
    }
}
