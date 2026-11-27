using System.Net;
using Freelance.ApiGateway.Services;
using Freelance.ApiGateway.UnitTests.TestData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.ApiGateway.UnitTests.Services;

public sealed class OpenApiAggregatorServiceTests
{
    private const string AuthUrl = "http://auth/openapi/v1.json";
    private const string ProjectsUrl = "http://projects/openapi/v1.json";
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    private static IConfiguration Configuration(bool withBaseUrl = true)
    {
        var values = new Dictionary<string, string?>
        {
            ["SwaggerEndPoints:0:Key"] = "auth",
            ["SwaggerEndPoints:0:Config:0:Name"] = "Auth API",
            ["SwaggerEndPoints:0:Config:0:Url"] = AuthUrl,
            ["SwaggerEndPoints:1:Key"] = "projects",
            ["SwaggerEndPoints:1:Config:0:Name"] = "Projects API",
            ["SwaggerEndPoints:1:Config:0:Url"] = ProjectsUrl,
            ["Routes:0:SwaggerKey"] = "auth",
            ["Routes:0:DownstreamPathTemplate"] = "/auth/{everything}",
            ["Routes:0:UpstreamPathTemplate"] = "/gateway-auth/{everything}"
        };
        if (withBaseUrl) values["GlobalConfiguration:BaseUrl"] = "http://gateway:9000";
        return new ConfigurationBuilder().AddInMemoryCollection(values).Build();
    }

    private static OpenApiAggregatorService Create(Dictionary<string, (HttpStatusCode, string)> responses, bool withBaseUrl = true) =>
        new(new HttpClient(OpenApiDocuments.Handler(responses).Object), Configuration(withBaseUrl), NullLogger<OpenApiAggregatorService>.Instance);

    [Fact]
    public async Task Aggregate_MergesPathsSchemasTagsAndSecurity_FromAllServices()
    {
        var service = Create(new()
        {
            [AuthUrl] = (HttpStatusCode.OK, OpenApiDocuments.Service("/auth/current", "Auth", "AccountDto")),
            [ProjectsUrl] = (HttpStatusCode.OK, OpenApiDocuments.Service("/project", "Project", "ProjectDto"))
        });

        var document = await service.AggregateOpenApiDocsAsync(_ct);

        document.Info.Title.Should().Be("Freelance API Gateway");
        document.Servers!.Should().ContainSingle().Which.Url.Should().Be("http://gateway:9000");
        document.Paths.Keys.Should().BeEquivalentTo("/gateway-auth/current", "/project");
        document.Components!.Schemas!.Keys.Should().BeEquivalentTo("AccountDto", "ProjectDto");
        document.Components.SecuritySchemes!.Keys.Should().ContainSingle().Which.Should().Be("Bearer");
        document.Tags!.Select(t => t.Name).Should().BeEquivalentTo("Auth", "Project");
        document.Security.Should().ContainSingle();
    }

    [Fact]
    public async Task Aggregate_SkipsServicesThatFailOrAreUnreachable()
    {
        var service = Create(new()
        {
            [AuthUrl] = (HttpStatusCode.ServiceUnavailable, "")
            // ProjectsUrl is not configured in the mock: the handler throws HttpRequestException.
        });

        var document = await service.AggregateOpenApiDocsAsync(_ct);

        document.Paths.Should().BeEmpty();
    }

    [Fact]
    public async Task Aggregate_DuplicatePaths_KeepTheFirstService()
    {
        var service = Create(new()
        {
            [AuthUrl] = (HttpStatusCode.OK, OpenApiDocuments.Service("/shared", "Auth", "A")),
            [ProjectsUrl] = (HttpStatusCode.OK, OpenApiDocuments.Service("/shared", "Project", "B"))
        });

        var document = await service.AggregateOpenApiDocsAsync(_ct);

        document.Paths.Should().ContainSingle();
        document.Paths["/shared"].Operations!.Values.Single().Tags!.Single().Name.Should().Be("Auth");
    }

    [Fact]
    public async Task Aggregate_InvalidDocument_IsSkipped()
    {
        var service = Create(new()
        {
            [AuthUrl] = (HttpStatusCode.OK, "{ \"openapi\": \"3.1.1\", \"paths\": 42 }"),
            [ProjectsUrl] = (HttpStatusCode.OK, OpenApiDocuments.Service("/project", "Project", "ProjectDto"))
        });

        var document = await service.AggregateOpenApiDocsAsync(_ct);

        document.Paths.Keys.Should().Equal("/project");
    }

    [Fact]
    public async Task Aggregate_WithoutBaseUrl_UsesLocalDefault() =>
        (await Create([], withBaseUrl: false).AggregateOpenApiDocsAsync(_ct)).Servers!.Single().Url.Should().Be("http://localhost:9000");
}
