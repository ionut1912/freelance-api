using System.Net;
using Freelance.ApiGateway.Services;
using Freelance.ApiGateway.UnitTests.TestData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Moq.Protected;

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

    [Fact]
    public async Task Aggregate_DocumentWithDiagnostics_IsStillMerged()
    {
        // Unknown non-extension properties are reported as diagnostics but the document is still usable.
        const string withDiagnostics =
            """{ "openapi": "3.1.1", "info": { "title": "x", "version": "1" }, "unexpected": true, "paths": { "/diag": { "get": { "responses": { "200": { "description": "OK" } } } } } }""";
        var service = Create(new() { [AuthUrl] = (HttpStatusCode.OK, withDiagnostics) });

        var document = await service.AggregateOpenApiDocsAsync(_ct);

        document.Paths.Keys.Should().Equal("/diag");
    }

    [Fact]
    public async Task Aggregate_ServiceTimingOutWhileSendingTheDocument_IsSkipped()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage request, CancellationToken _) => request.RequestUri!.ToString() == AuthUrl
                ? new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(new TimingOutStream()) }
                : new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(OpenApiDocuments.Service("/project", "Project", "ProjectDto"))
                });
        var service = new OpenApiAggregatorService(new HttpClient(handler.Object), Configuration(),
            NullLogger<OpenApiAggregatorService>.Instance);

        var document = await service.AggregateOpenApiDocsAsync(_ct);

        document.Paths.Keys.Should().Equal("/project");
    }

    /// <summary>Simulates HttpClient's timeout firing while the response body is being read.</summary>
    private sealed class TimingOutStream : Stream
    {
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => 0; set => throw new NotSupportedException(); }
        public override int Read(byte[] buffer, int offset, int count) => throw new TaskCanceledException("timeout");
        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) =>
            ValueTask.FromException<int>(new TaskCanceledException("timeout"));
        public override void Flush() { }
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }

    [Fact]
    public async Task Aggregate_ResponseStreamDisposedAsynchronously_IsMerged()
    {
        var body = System.Text.Encoding.UTF8.GetBytes(OpenApiDocuments.Service("/async", "Async", "AsyncDto"));
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(new AsyncDisposingStream(body)) });
        var service = new OpenApiAggregatorService(new HttpClient(handler.Object), Configuration(),
            NullLogger<OpenApiAggregatorService>.Instance);

        var document = await service.AggregateOpenApiDocsAsync(_ct);

        document.Paths.Keys.Should().Contain("/async");
    }

    /// <summary>Like a network stream, completes DisposeAsync asynchronously.</summary>
    private sealed class AsyncDisposingStream(byte[] buffer) : MemoryStream(buffer)
    {
        public override async ValueTask DisposeAsync()
        {
            await Task.Yield();
            await base.DisposeAsync();
        }
    }

    [Fact]
    public async Task Aggregate_MalformedJson_DoesNotAffectTheOtherServices()
    {
        var service = Create(new()
        {
            [AuthUrl] = (HttpStatusCode.OK, "{ this is not json"),
            [ProjectsUrl] = (HttpStatusCode.OK, OpenApiDocuments.Service("/project", "Project", "ProjectDto"))
        });

        var document = await service.AggregateOpenApiDocsAsync(_ct);

        document.Paths.Keys.Should().Equal("/project");
    }

    [Fact]
    public async Task Aggregate_AllServicesUnreachable_ReturnsTheBaseDocument()
    {
        var document = await Create([]).AggregateOpenApiDocsAsync(_ct);

        document.Paths.Should().BeEmpty();
        document.Info.Title.Should().Be("Freelance API Gateway");
        document.Components!.SecuritySchemes.Should().ContainKey("Bearer");
    }

    [Fact]
    public async Task Aggregate_WithoutAnyConfiguration_ReturnsTheBaseDocument()
    {
        var service = new OpenApiAggregatorService(new HttpClient(OpenApiDocuments.Handler(
                new Dictionary<string, (HttpStatusCode, string)>()).Object),
            new ConfigurationBuilder().Build(), NullLogger<OpenApiAggregatorService>.Instance);

        var document = await service.AggregateOpenApiDocsAsync(_ct);

        document.Paths.Should().BeEmpty();
    }

    [Theory]
    [InlineData("""{ "openapi": "3.1.1", "info": { "title": "x", "version": "1" } }""")]
    [InlineData("""{ "openapi": "3.1.1", "info": { "title": "x", "version": "1" }, "paths": {}, "components": {} }""")]
    public async Task Aggregate_DocumentWithoutOptionalSections_AddsNothing(string json)
    {
        var service = Create(new() { [AuthUrl] = (HttpStatusCode.OK, json) });

        var document = await service.AggregateOpenApiDocsAsync(_ct);

        document.Paths.Should().BeEmpty();
        document.Components!.Schemas.Should().BeEmpty();
        document.Tags.Should().BeEmpty();
    }
}
