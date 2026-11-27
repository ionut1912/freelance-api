using System.Net;
using System.Net.Http.Json;
using Freelance.UserProfiles.Application.Dtos;

namespace Freelance.UserProfiles.IntegrationTests.Endpoints;

/// <summary>A caller may only change the profile that belongs to their own account.</summary>
public sealed class ProfileOwnershipTests(UserProfilesApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    private static object Body(string action) => action switch
    {
        "address" => new { addressDto = new AddressDto("Other", "Cluj", "CJ", "400000", "RO", "2") },
        "data" => new { bio = "Hijacked", image = "hijacked" },
        _ => new
        {
            foreignLanguages = new[] { "German" },
            programmingLanguages = new[] { "Rust" },
            areas = new[] { "Systems" },
            experience = "1y",
            amount = 1,
            currency = "EUR",
            portfolioUrl = "https://evil.io"
        }
    };

    [Theory]
    [InlineData("address")]
    [InlineData("data")]
    public async Task UpdateClientProfile_OfAnotherAccount_ReturnsNotFoundAndKeepsIt(string action)
    {
        var owner = factory.CreateClient("Client");
        var profile = await UserProfilesApiFactory.CreateClientProfileAsync(owner, _ct);

        var response = await factory.CreateClient("Client")
            .PutAsJsonAsync($"/client-profiles/{profile.Id}/{action}", Body(action), _ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await owner.GetFromJsonAsync<ClientProfileDto>("/client-profiles/current", _ct)).Should().BeEquivalentTo(profile);
    }

    [Fact]
    public async Task DeleteClientProfile_OfAnotherAccount_ReturnsNotFoundAndKeepsIt()
    {
        var owner = factory.CreateClient("Client");
        var profile = await UserProfilesApiFactory.CreateClientProfileAsync(owner, _ct);

        var response = await factory.CreateClient("Client").DeleteAsync($"/client-profiles/{profile.Id}", _ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await owner.GetAsync("/client-profiles/current", _ct)).StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("address")]
    [InlineData("data")]
    [InlineData("details")]
    public async Task UpdateFreelancerProfile_OfAnotherAccount_ReturnsNotFoundAndKeepsIt(string action)
    {
        var owner = factory.CreateClient("Freelancer");
        var profile = await UserProfilesApiFactory.CreateFreelancerProfileAsync(owner, _ct);

        var response = await factory.CreateClient("Freelancer")
            .PutAsJsonAsync($"/freelancer-profiles/{action}/{profile.Id}", Body(action), _ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await owner.GetFromJsonAsync<FreelancerProfileDto>("/freelancer-profiles/current", _ct))
            .Should().BeEquivalentTo(profile);
    }

    [Fact]
    public async Task DeleteFreelancerProfile_OfAnotherAccount_ReturnsNotFoundAndKeepsIt()
    {
        var owner = factory.CreateClient("Freelancer");
        var profile = await UserProfilesApiFactory.CreateFreelancerProfileAsync(owner, _ct);

        var response = await factory.CreateClient("Freelancer").DeleteAsync($"/freelancer-profiles/{profile.Id}", _ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await owner.GetAsync("/freelancer-profiles/current", _ct)).StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
