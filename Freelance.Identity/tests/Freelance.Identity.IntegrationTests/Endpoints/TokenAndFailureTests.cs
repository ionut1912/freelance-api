using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Freelance.Identity.Domain.interfaces;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Freelance.Identity.IntegrationTests.Endpoints;

public sealed class TokenAndFailureTests(IdentityApiFactory factory)
{
    // Matches JwtSettings in appsettings.Development.json.
    private const string Key = "YourSuperSecretKeyHere123!ChangeItTo32PlusChars";
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    private static string Token(string key = Key, string issuer = "FreelanceApp", string audience = "FreelanceAppUsers",
        int expiresInMinutes = 30)
    {
        var expires = DateTime.UtcNow.AddMinutes(expiresInMinutes);
        return new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            IssuedAt = expires.AddHours(-1),
            NotBefore = expires.AddHours(-1),
            Expires = expires,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256),
            Claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = Guid.NewGuid().ToString(),
                [ClaimTypes.Name] = "nobody"
            }
        });
    }

    private async Task<HttpStatusCode> GetCurrentAsync(string token)
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return (await client.GetAsync("/auth/current", _ct)).StatusCode;
    }

    [Fact]
    public async Task ProtectedEndpoint_ValidTokenForUnknownAccount_ReturnsNotFound() =>
        (await GetCurrentAsync(Token())).Should().Be(HttpStatusCode.NotFound);

    public static TheoryData<string> RejectedTokens => new()
    {
        Token(key: "another-signing-key-that-is-long-enough-1234"),
        Token(issuer: "someone-else"),
        Token(audience: "someone-else"),
        Token(expiresInMinutes: -10),
        "not-a-token"
    };

    [Theory]
    [MemberData(nameof(RejectedTokens), DisableDiscoveryEnumeration = true)]
    public async Task ProtectedEndpoint_InvalidToken_ReturnsUnauthorized(string token) =>
        (await GetCurrentAsync(token)).Should().Be(HttpStatusCode.Unauthorized);

    [Fact]
    public async Task Login_WhenTheRepositoryFails_Returns500WithoutLeakingDetails()
    {
        var repository = new Mock<IAccountRepository>();
        repository.Setup(r => r.GetAccountByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("connection string: Host=secret-db"));
        await using var failing = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IAccountRepository>();
            services.AddScoped(_ => repository.Object);
        }));

        var response = await failing.CreateClient()
            .PostAsJsonAsync("/auth/login", new { username = "x", password = "y" }, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        (await response.Content.ReadAsStringAsync(_ct)).Should()
            .NotContain("secret-db").And.NotContain("InvalidOperationException").And.NotContain(" at ");
    }

    [Theory]
    [InlineData("{ not json")]
    [InlineData("")]
    public async Task Register_MalformedBody_ReturnsClientError(string body)
    {
        var response = await factory.CreateClient()
            .PostAsync("/auth/register", new StringContent(body, Encoding.UTF8, "application/json"), _ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
