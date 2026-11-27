using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Freelance.UserProfiles.Application.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Testcontainers.PostgreSql;
using Wolverine;

[assembly: AssemblyFixture(typeof(Freelance.UserProfiles.IntegrationTests.UserProfilesApiFactory))]

namespace Freelance.UserProfiles.IntegrationTests;

/// <summary>
/// Boots the real UserProfiles API against a throw-away PostgreSQL container (EF Core + Wolverine durable outbox).
/// RabbitMQ is replaced by Wolverine's stubbed transports, so published messages can be asserted with tracked sessions.
/// </summary>
public sealed class UserProfilesApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // Must match JwtSettings in appsettings.Development.json (tokens are normally issued by the Identity service).
    private const string JwtKey = "YourSuperSecretKeyHere123!ChangeItTo32PlusChars";

    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine").Build();

    public static readonly AddressDto Address = new("Main", "Bucharest", "B", "010101", "RO", "10");

    public IHost Host => Services.GetRequiredService<IHost>();

    public async ValueTask InitializeAsync()
    {
        await _postgres.StartAsync();
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", _postgres.GetConnectionString());
        Environment.SetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT", "http://localhost:4317");
        Environment.SetEnvironmentVariable("OTEL_EXPORTER_OTLP_LOGS_ENDPOINT", "http://localhost:3100");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureTestServices(services =>
        {
            services.DisableAllExternalWolverineTransports();
            services.RunWolverineInSoloMode();
        });
    }

    public static string CreateToken(string role, Guid accountId) =>
        new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = "FreelanceApp",
            Audience = "FreelanceAppUsers",
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey)), SecurityAlgorithms.HmacSha256),
            Claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = accountId.ToString(),
                [ClaimTypes.Role] = role
            }
        });

    public HttpClient CreateClient(string role, Guid? accountId = null)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(role, accountId ?? Guid.NewGuid()));
        return client;
    }

    public static async Task<ClientProfileDto> CreateClientProfileAsync(HttpClient client, CancellationToken ct)
    {
        var response = await client.PostAsJsonAsync("/client-profiles", new { address = Address, bio = "Client bio", image = "data:image/png;base64,AAAA" }, ct);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ClientProfileDto>(ct))!;
    }

    public static async Task<FreelancerProfileDto> CreateFreelancerProfileAsync(HttpClient client, CancellationToken ct)
    {
        var response = await client.PostAsJsonAsync("/freelancer-profiles", new
        {
            address = Address,
            bio = "Dev",
            image = "data:image/png;base64,AAAA",
            experience = "5y",
            amount = 50,
            currency = "EUR",
            portfolioUrl = "https://x.io",
            foreignLanguages = new[] { "English", "French" },
            programmingLanguages = new[] { "C#", "Go" },
            areas = new[] { "Backend", "Backend" }
        }, ct);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<FreelancerProfileDto>(ct))!;
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _postgres.DisposeAsync();
    }
}
