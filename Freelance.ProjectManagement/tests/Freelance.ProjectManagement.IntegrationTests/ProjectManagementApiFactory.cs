using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Testcontainers.PostgreSql;

[assembly: AssemblyFixture(typeof(Freelance.ProjectManagement.IntegrationTests.ProjectManagementApiFactory))]

namespace Freelance.ProjectManagement.IntegrationTests;

/// <summary>Boots the real ProjectManagement API against a throw-away PostgreSQL container.</summary>
public sealed class ProjectManagementApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // Must match JwtSettings in appsettings.Development.json (tokens are normally issued by the Identity service).
    private const string JwtKey = "YourSuperSecretKeyHere123!ChangeItTo32PlusChars";
    private const string JwtIssuer = "FreelanceApp";
    private const string JwtAudience = "FreelanceAppUsers";

    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine").Build();

    public async ValueTask InitializeAsync()
    {
        await _postgres.StartAsync();
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", _postgres.GetConnectionString());
        Environment.SetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT", "http://localhost:4317");
        Environment.SetEnvironmentVariable("OTEL_EXPORTER_OTLP_LOGS_ENDPOINT", "http://localhost:3100");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.UseEnvironment("Development");

    public static string CreateToken(string role, Guid? accountId = null) =>
        new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = JwtIssuer,
            Audience = JwtAudience,
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey)),
                SecurityAlgorithms.HmacSha256),
            Claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = (accountId ?? Guid.NewGuid()).ToString(),
                [ClaimTypes.Role] = role
            }
        });

    public HttpClient CreateClient(string role)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(role));
        return client;
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _postgres.DisposeAsync();
    }
}
