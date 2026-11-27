using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Freelance.ProjectManagement.IntegrationTests.Endpoints;

/// <summary>A correctly signed token that carries no account id is rejected by every endpoint that needs one.</summary>
public sealed class MissingAccountClaimTests(ProjectManagementApiFactory factory)
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
    [InlineData("Client", "POST", "/project")]
    [InlineData("Client", "PUT", "/project/11111111-1111-1111-1111-111111111111")]
    [InlineData("Client", "DELETE", "/project/11111111-1111-1111-1111-111111111111")]
    [InlineData("Client", "POST", "/projectTask")]
    [InlineData("Client", "PUT", "/projectTask/11111111-1111-1111-1111-111111111111")]
    [InlineData("Client", "DELETE", "/projectTask/11111111-1111-1111-1111-111111111111")]
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
