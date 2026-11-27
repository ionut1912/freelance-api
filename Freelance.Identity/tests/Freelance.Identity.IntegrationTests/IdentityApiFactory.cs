using System.Net.Http.Headers;
using System.Net.Http.Json;
using Freelance.Identity.Application.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

[assembly: AssemblyFixture(typeof(Freelance.Identity.IntegrationTests.IdentityApiFactory))]

namespace Freelance.Identity.IntegrationTests;

/// <summary>Boots the real Identity API (Program) against a throw-away PostgreSQL container.</summary>
public sealed class IdentityApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public const string Password = "P@ssw0rd!";

    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine").Build();

    public async ValueTask InitializeAsync()
    {
        await _postgres.StartAsync();

        // Program reads configuration while building the host, so environment variables are the reliable override.
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", _postgres.GetConnectionString());
        Environment.SetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT", "http://localhost:4317");
        Environment.SetEnvironmentVariable("OTEL_EXPORTER_OTLP_LOGS_ENDPOINT", "http://localhost:3100");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.UseEnvironment("Development");

    public static string NewUsername() => $"user{Guid.NewGuid():N}"[..20];

    public async Task<HttpResponseMessage> RegisterAsync(HttpClient client, string username, string role = "Client") =>
        await client.PostAsJsonAsync("/auth/register", new
        {
            email = $"{username}@test.io",
            password = Password,
            phoneNumber = "0700000000",
            username,
            role
        });

    /// <summary>Registers a user, logs in and returns a client authenticated with the issued JWT.</summary>
    public async Task<(HttpClient Client, AccountDto Account)> CreateAuthenticatedClientAsync(string? username = null)
    {
        username ??= NewUsername();
        var client = CreateClient();
        (await RegisterAsync(client, username)).EnsureSuccessStatusCode();

        var login = await client.PostAsJsonAsync("/auth/login", new { username, password = Password });
        login.EnsureSuccessStatusCode();
        var account = (await login.Content.ReadFromJsonAsync<AccountDto>())!;

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", account.Token);
        return (client, account);
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _postgres.DisposeAsync();
    }
}
