using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Freelance.UserProfiles.IntegrationTests.Endpoints;

/// <summary>A correctly signed token that carries no account id is rejected by every endpoint that needs one.</summary>
public sealed class MissingAccountClaimTests(UserProfilesApiFactory factory)
{
    private static string TokenWithoutAccountId(string role) =>
        new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = "FreelanceApp",
            Audience = "FreelanceAppUsers",
            Expires = DateTime.UtcNow.AddMinutes(30),
            // Matches JwtSettings in appsettings.Development.json.
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKeyHere123!ChangeItTo32PlusChars")),
                SecurityAlgorithms.HmacSha256),
            Claims = new Dictionary<string, object> { [ClaimTypes.Role] = role }
        });

    [Theory]
    [InlineData("Client", "POST", "/client-profiles")]
    [InlineData("Client", "GET", "/client-profiles/current")]
    [InlineData("Client", "PUT", "/client-profiles/verify")]
    [InlineData("Client", "DELETE", "/client-profiles/11111111-1111-1111-1111-111111111111")]
    [InlineData("Freelancer", "POST", "/freelancer-profiles")]
    [InlineData("Freelancer", "GET", "/freelancer-profiles/current")]
    [InlineData("Freelancer", "PUT", "/freelancer-profiles/verify")]
    [InlineData("Freelancer", "DELETE", "/freelancer-profiles/11111111-1111-1111-1111-111111111111")]
    [InlineData("Client", "PUT", "/client-profiles/11111111-1111-1111-1111-111111111111/address")]
    [InlineData("Client", "PUT", "/client-profiles/11111111-1111-1111-1111-111111111111/data")]
    [InlineData("Freelancer", "PUT", "/freelancer-profiles/details/11111111-1111-1111-1111-111111111111")]
    [InlineData("Freelancer", "PUT", "/freelancer-profiles/address/11111111-1111-1111-1111-111111111111")]
    [InlineData("Freelancer", "PUT", "/freelancer-profiles/data/11111111-1111-1111-1111-111111111111")]
    public async Task Endpoint_TokenWithoutAccountId_ReturnsUnauthorized(string role, string method, string url)
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenWithoutAccountId(role));
        var request = new HttpRequestMessage(new HttpMethod(method), url);
        if (method is "POST" or "PUT") request.Content = JsonContent.Create(new { });

        var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
