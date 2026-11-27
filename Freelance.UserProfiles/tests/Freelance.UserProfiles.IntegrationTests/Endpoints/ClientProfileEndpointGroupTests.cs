using System.Net;
using System.Net.Http.Json;
using Freelance.UserProfiles.Application.Dtos;

namespace Freelance.UserProfiles.IntegrationTests.Endpoints;

public sealed class ClientProfileEndpointGroupTests(UserProfilesApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task Create_ReturnsProfile_WithCorrectlyMappedAddress()
    {
        var accountId = Guid.NewGuid();
        var client = factory.CreateClient("Client", accountId);

        var profile = await UserProfilesApiFactory.CreateClientProfileAsync(client, _ct);

        profile.Should().BeEquivalentTo(new { AccountId = accountId, Address = UserProfilesApiFactory.Address, IsVerified = false });
    }

    [Fact]
    public async Task Create_Twice_ReturnsBadRequest()
    {
        var client = factory.CreateClient("Client");
        await UserProfilesApiFactory.CreateClientProfileAsync(client, _ct);

        (await client.PostAsJsonAsync("/client-profiles", new { address = UserProfilesApiFactory.Address, bio = "b", image = "i" }, _ct))
            .StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Invalid_ReturnsValidationProblem() =>
        (await factory.CreateClient("Client").PostAsJsonAsync("/client-profiles", new { bio = "", image = "" }, _ct))
        .StatusCode.Should().Be(HttpStatusCode.BadRequest);

    [Fact]
    public async Task ClientOnlyEndpoints_AsFreelancer_AreForbidden_ButListingIsAllowed()
    {
        var freelancer = factory.CreateClient("Freelancer");

        (await freelancer.GetAsync("/client-profiles/current", _ct)).StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await freelancer.GetAsync("/client-profiles", _ct)).StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Endpoints_WithoutToken_ReturnUnauthorized() =>
        (await factory.CreateClient().GetAsync("/client-profiles", _ct)).StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    [Fact]
    public async Task Current_WithoutProfile_ReturnsNotFound() =>
        (await factory.CreateClient("Client").GetAsync("/client-profiles/current", _ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);

    [Fact]
    public async Task UpdateAddressAndData_ArePersisted()
    {
        var client = factory.CreateClient("Client");
        var profile = await UserProfilesApiFactory.CreateClientProfileAsync(client, _ct);

        (await client.PutAsJsonAsync($"/client-profiles/{profile.Id}/address",
            new { addressDto = new AddressDto("Second", "Cluj", "CJ", "400000", "RO", "2") }, _ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await client.PutAsJsonAsync($"/client-profiles/{profile.Id}/data", new { bio = "New bio", image = "new" }, _ct))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await client.PutAsJsonAsync($"/client-profiles/{profile.Id}/data", new { bio = "New bio", image = "other" }, _ct))
            .StatusCode.Should().Be(HttpStatusCode.BadRequest, "the bio did not change");

        var current = await client.GetFromJsonAsync<ClientProfileDto>("/client-profiles/current", _ct);
        current.Should().BeEquivalentTo(new { Bio = "New bio", Image = "new" });
        current!.Address.City.Should().Be("Cluj");
        (await client.GetFromJsonAsync<List<ClientProfileDto>>("/client-profiles", _ct)).Should().Contain(p => p.Id == profile.Id);
    }

    [Fact]
    public async Task Verify_WithoutProfile_ReturnsNotFound() =>
        (await factory.CreateClient("Client").PutAsJsonAsync("/client-profiles/verify", new { imageUrl = "x" }, _ct))
        .StatusCode.Should().Be(HttpStatusCode.NotFound);

    [Fact]
    public async Task Delete_RemovesProfile()
    {
        var client = factory.CreateClient("Client");
        var profile = await UserProfilesApiFactory.CreateClientProfileAsync(client, _ct);

        (await client.DeleteAsync($"/client-profiles/{profile.Id}", _ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await client.GetAsync("/client-profiles/current", _ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
