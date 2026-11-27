using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Freelance.ApiGateway.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Moq.Protected;
using Ocelot.Requester;

[assembly: AssemblyFixture(typeof(Freelance.ApiGateway.IntegrationTests.GatewayFactory))]

namespace Freelance.ApiGateway.IntegrationTests;

/// <summary>
/// Boots the real gateway (Ocelot + aggregated OpenAPI). Downstream services are replaced by:
/// a global Ocelot delegating handler that echoes the forwarded request, and a mocked HttpClient handler
/// serving the downstream OpenAPI documents.
/// </summary>
public sealed class GatewayFactory : WebApplicationFactory<Program>
{
    private const string JwtKey = "YourSuperSecretKeyHere123!ChangeItTo32PlusChars";

    public Mock<HttpMessageHandler> OpenApiHandler { get; } = CreateOpenApiHandler();

    private static Mock<HttpMessageHandler> CreateOpenApiHandler()
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage request, CancellationToken _) => request.RequestUri!.Host switch
            {
                "freelance_identity" => Json(HttpStatusCode.OK, Document("/auth/login", "Auth")),
                "freelance_project_management" => Json(HttpStatusCode.OK, Document("/project", "Project")),
                _ => new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            });
        return handler;
    }

    private static HttpResponseMessage Json(HttpStatusCode status, string body) =>
        new(status) { Content = new StringContent(body, Encoding.UTF8, "application/json") };

    private static string Document(string path, string tag) =>
        $$"""{"openapi":"3.1.1","info":{"title":"{{tag}}","version":"v1"},"paths":{"{{path}}":{"post":{"tags":["{{tag}}"],"responses":{"200":{"description":"OK"} } } } } }""";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.UseSetting("OTEL_EXPORTER_OTLP_ENDPOINT", "http://localhost:4317");
        builder.UseSetting("OTEL_EXPORTER_OTLP_LOGS_ENDPOINT", "http://localhost:3100");
        builder.ConfigureTestServices(services =>
        {
            services.AddHttpClient<OpenApiAggregatorService>().ConfigurePrimaryHttpMessageHandler(() => OpenApiHandler.Object);
            // Equivalent of AddOcelot().AddDelegatingHandler<EchoDownstreamHandler>(global: true).
            services.AddTransient<EchoDownstreamHandler>();
            services.AddTransient(sp => new GlobalDelegatingHandler(sp.GetRequiredService<EchoDownstreamHandler>()));
        });
    }

    public static string CreateToken(string role = "Client") =>
        new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = "FreelanceApp",
            Audience = "FreelanceAppUsers",
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey)), SecurityAlgorithms.HmacSha256),
            Claims = new Dictionary<string, object> { [JwtRegisteredClaimNames.Sub] = Guid.NewGuid().ToString(), [ClaimTypes.Role] = role }
        });

    public HttpClient CreateAuthenticatedClient()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken());
        return client;
    }
}

/// <summary>Short-circuits Ocelot's downstream call and echoes what would have been sent.</summary>
public sealed class EchoDownstreamHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var echo = JsonSerializer.Serialize(new DownstreamEcho(request.Method.Method, request.RequestUri!.ToString(),
            request.Headers.Authorization is not null));
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(echo, Encoding.UTF8, "application/json")
        });
    }
}

public sealed record DownstreamEcho(string Method, string Url, bool HasAuthorization);
