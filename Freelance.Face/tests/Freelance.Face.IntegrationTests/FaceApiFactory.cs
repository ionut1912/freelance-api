using System.Net.Http.Headers;
using System.Text;
using Freelance.Face.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Wolverine;

[assembly: AssemblyFixture(typeof(Freelance.Face.IntegrationTests.FaceApiFactory))]

namespace Freelance.Face.IntegrationTests;

/// <summary>
/// Boots the real Face API. RabbitMQ is replaced by Wolverine's stubbed transports and the dlib service by a mock,
/// so the HTTP surface and the message flow are tested without native models or a broker.
/// </summary>
public sealed class FaceApiFactory : WebApplicationFactory<Program>
{
    public const string FrontendUrl = "https://frontend.test/";

    public Mock<IFaceService> FaceService { get; } = new();

    public IHost Host => Services.GetRequiredService<IHost>();

    public HttpClient CreateAuthenticatedClient() => Authenticate(CreateClient());

    /// <summary>Adds a token like the ones Identity issues, signed with the key from appsettings.Development.json.</summary>
    public static HttpClient Authenticate(HttpClient client)
    {
        var token = new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = "FreelanceApp",
            Audience = "FreelanceAppUsers",
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKeyHere123!ChangeItTo32PlusChars")),
                SecurityAlgorithms.HmacSha256),
            Claims = new Dictionary<string, object> { [JwtRegisteredClaimNames.Sub] = Guid.NewGuid().ToString() }
        });
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.UseSetting("FRONTEND_BASE_URL", FrontendUrl);
        builder.UseSetting("OTEL_EXPORTER_OTLP_ENDPOINT", "http://localhost:4317");
        builder.UseSetting("OTEL_EXPORTER_OTLP_LOGS_ENDPOINT", "http://localhost:3100");
        builder.ConfigureTestServices(services =>
        {
            services.DisableAllExternalWolverineTransports();
            services.RemoveAll<IFaceService>();
            services.AddSingleton(FaceService.Object);
        });
    }
}
