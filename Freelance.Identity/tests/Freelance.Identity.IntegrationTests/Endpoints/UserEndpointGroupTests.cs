using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Freelance.Identity.Application.Dtos;

namespace Freelance.Identity.IntegrationTests.Endpoints;

public sealed class UserEndpointGroupTests(IdentityApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task Register_ReturnsCreatedAccount_WithLocationAndWithoutPassword()
    {
        var client = factory.CreateClient();
        var username = IdentityApiFactory.NewUsername();

        var response = await factory.RegisterAsync(client, username, "Freelancer");

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadAsStringAsync(_ct);
        body.Should().NotContainEquivalentOf("password");
        var dto = JsonSerializer.Deserialize<AccountDto>(body, JsonSerializerOptions.Web)!;
        dto.Should().BeEquivalentTo(new { Username = username, Role = "Freelancer", IsBlocked = false, Token = (string?)null });
        response.Headers.Location!.OriginalString.Should().Be($"/auth/{dto.Id}");
    }

    [Fact]
    public async Task Register_InvalidPayload_ReturnsValidationProblem()
    {
        var response = await factory.CreateClient().PostAsJsonAsync("/auth/register",
            new { email = "nope", password = "", phoneNumber = "0700000000", username = IdentityApiFactory.NewUsername(), role = "Admin" }, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        using var problem = JsonDocument.Parse(await response.Content.ReadAsStringAsync(_ct));
        problem.RootElement.GetProperty("title").GetString().Should().Be("Validation Error");
        problem.RootElement.GetProperty("errors").EnumerateArray()
            .Select(e => e.GetProperty("property").GetString())
            .Should().Contain(["Email", "Password", "Role"]);
    }

    [Fact]
    public async Task Register_DuplicateUsername_ReturnsBadRequest()
    {
        var client = factory.CreateClient();
        var username = IdentityApiFactory.NewUsername();
        (await factory.RegisterAsync(client, username)).EnsureSuccessStatusCode();

        var response = await factory.RegisterAsync(client, username);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await response.Content.ReadAsStringAsync(_ct)).Should().Contain("User Already Exists");
    }

    [Fact]
    public async Task Login_ReturnsToken_AndCurrentReturnsTheAccount()
    {
        var (client, account) = await factory.CreateAuthenticatedClientAsync();

        account.Token.Should().NotBeNullOrWhiteSpace();
        var current = await client.GetFromJsonAsync<AccountDto>("/auth/current", _ct);
        current!.Id.Should().Be(account.Id);
        current.Token.Should().BeNull();
    }

    [Fact]
    public async Task Login_WrongPassword_ReturnsBadRequest()
    {
        var client = factory.CreateClient();
        var username = IdentityApiFactory.NewUsername();
        (await factory.RegisterAsync(client, username)).EnsureSuccessStatusCode();

        var response = await client.PostAsJsonAsync("/auth/login", new { username, password = "wrong" }, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_UnknownUser_ReturnsNotFound() =>
        (await factory.CreateClient().PostAsJsonAsync("/auth/login",
            new { username = IdentityApiFactory.NewUsername(), password = "x" }, _ct))
        .StatusCode.Should().Be(HttpStatusCode.NotFound);

    [Theory]
    [InlineData("GET", "/auth/current")]
    [InlineData("PUT", "/auth/block")]
    [InlineData("PUT", "/auth/unblock")]
    [InlineData("DELETE", "/auth/")]
    public async Task ProtectedEndpoints_WithoutToken_ReturnUnauthorized(string method, string url) =>
        (await factory.CreateClient().SendAsync(new HttpRequestMessage(new HttpMethod(method), url), _ct))
        .StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    [Fact]
    public async Task Block_PreventsLogin_AndUnblockRestoresIt()
    {
        var username = IdentityApiFactory.NewUsername();
        var (client, _) = await factory.CreateAuthenticatedClientAsync(username);

        (await client.PutAsync("/auth/block", null, _ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);
        var blocked = await client.PostAsJsonAsync("/auth/login", new { username, password = IdentityApiFactory.Password }, _ct);
        blocked.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await blocked.Content.ReadAsStringAsync(_ct)).Should().Contain("Account Blocked");

        (await client.PutAsync("/auth/block", null, _ct)).StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await client.PutAsync("/auth/unblock", null, _ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await client.PutAsync("/auth/unblock", null, _ct)).StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await client.PostAsJsonAsync("/auth/login", new { username, password = IdentityApiFactory.Password }, _ct))
            .StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_RemovesTheAccount()
    {
        var username = IdentityApiFactory.NewUsername();
        var (client, _) = await factory.CreateAuthenticatedClientAsync(username);

        (await client.DeleteAsync("/auth/", _ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);

        (await client.PostAsJsonAsync("/auth/login", new { username, password = IdentityApiFactory.Password }, _ct))
            .StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await client.GetAsync("/auth/current", _ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
