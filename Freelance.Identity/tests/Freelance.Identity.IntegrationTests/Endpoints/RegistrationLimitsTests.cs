using System.Net;
using System.Net.Http.Json;

namespace Freelance.Identity.IntegrationTests.Endpoints;

public sealed class RegistrationLimitsTests(IdentityApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    private Task<HttpResponseMessage> RegisterAsync(string username, string? email = null, string? password = null) =>
        factory.CreateClient().PostAsJsonAsync("/auth/register", new
        {
            email = email ?? $"{Guid.NewGuid():N}@test.io",
            password = password ?? IdentityApiFactory.Password,
            phoneNumber = "0700000000",
            username,
            role = "Client"
        }, _ct);

    [Fact]
    public async Task Register_UsernameLongerThanColumn_ReturnsBadRequest() =>
        (await RegisterAsync(new string('u', 51))).StatusCode.Should().Be(HttpStatusCode.BadRequest);

    [Fact]
    public async Task Register_EmailLongerThanColumn_ReturnsBadRequest() =>
        (await RegisterAsync(IdentityApiFactory.NewUsername(), email: $"{new string('e', 95)}@test.io"))
        .StatusCode.Should().Be(HttpStatusCode.BadRequest);

    /// <summary>BCrypt silently ignores everything after 72 bytes.</summary>
    [Fact]
    public async Task Register_PasswordBeyondBCryptLimit_ReturnsBadRequest() =>
        (await RegisterAsync(IdentityApiFactory.NewUsername(), password: new string('p', 73)))
        .StatusCode.Should().Be(HttpStatusCode.BadRequest);

    [Fact]
    public async Task Register_SameUsernameConcurrently_CreatesOneAccountAndRejectsTheRest()
    {
        var username = IdentityApiFactory.NewUsername();

        var responses = await Task.WhenAll(Enumerable.Range(0, 8).Select(_ => RegisterAsync(username)));

        var statuses = responses.Select(r => r.StatusCode).ToList();
        statuses.Count(s => s == HttpStatusCode.Created).Should().Be(1);
        statuses.Should().OnlyContain(s => s == HttpStatusCode.Created || s == HttpStatusCode.BadRequest);
    }
}
