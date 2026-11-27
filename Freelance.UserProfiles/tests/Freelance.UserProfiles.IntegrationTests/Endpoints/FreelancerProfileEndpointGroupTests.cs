using System.Net;
using System.Net.Http.Json;
using Freelance.UserProfiles.Application.Dtos;

namespace Freelance.UserProfiles.IntegrationTests.Endpoints;

public sealed class FreelancerProfileEndpointGroupTests(UserProfilesApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task Create_WithSeveralLanguagesAndSkills_ReturnsProfile()
    {
        var freelancer = factory.CreateClient("Freelancer");

        var profile = await UserProfilesApiFactory.CreateFreelancerProfileAsync(freelancer, _ct);

        profile.ForeignLanguages.Should().BeEquivalentTo("English", "French");
        profile.ProgrammingLanguages.Should().BeEquivalentTo("C#", "Go");
        (await freelancer.GetFromJsonAsync<FreelancerProfileDto>("/freelancer-profiles/current", _ct))!.Id.Should().Be(profile.Id);
    }

    [Fact]
    public async Task Create_Twice_ReturnsBadRequest()
    {
        var freelancer = factory.CreateClient("Freelancer");
        await UserProfilesApiFactory.CreateFreelancerProfileAsync(freelancer, _ct);

        await freelancer.Invoking(c => UserProfilesApiFactory.CreateFreelancerProfileAsync(c, _ct))
            .Should().ThrowAsync<HttpRequestException>().Where(e => e.StatusCode == HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateDetails_ReplacesLanguagesAndSkills()
    {
        var freelancer = factory.CreateClient("Freelancer");
        var profile = await UserProfilesApiFactory.CreateFreelancerProfileAsync(freelancer, _ct);

        var response = await freelancer.PutAsJsonAsync($"/freelancer-profiles/details/{profile.Id}", new
        {
            foreignLanguages = new[] { "English", "German" },
            programmingLanguages = new[] { "C#", "Rust" },
            areas = new[] { "Backend", "Systems" },
            experience = "6y",
            amount = 60,
            currency = "EUR",
            portfolioUrl = "https://y.io"
        }, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var updated = await freelancer.GetFromJsonAsync<FreelancerProfileDto>("/freelancer-profiles/current", _ct);
        updated!.ForeignLanguages.Should().BeEquivalentTo("English", "German");
        updated.ProgrammingLanguages.Should().BeEquivalentTo("C#", "Rust");
        updated.Should().BeEquivalentTo(new { Experience = "6y", Amount = 60m });
    }

    [Fact]
    public async Task UpdateAddressDataAndRating()
    {
        var freelancer = factory.CreateClient("Freelancer");
        var profile = await UserProfilesApiFactory.CreateFreelancerProfileAsync(freelancer, _ct);

        (await freelancer.PutAsJsonAsync($"/freelancer-profiles/address/{profile.Id}",
            new { addressDto = new AddressDto("S", "Iasi", "IS", "7", "RO", "3") }, _ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await freelancer.PutAsJsonAsync($"/freelancer-profiles/data/{profile.Id}", new { bio = "New", image = "new" }, _ct))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await freelancer.PutAsJsonAsync($"/freelancer-profiles/rating/{profile.Id}", new { rating = 5 }, _ct))
            .StatusCode.Should().Be(HttpStatusCode.Forbidden, "only clients rate freelancers");
        (await factory.CreateClient("Client").PutAsJsonAsync($"/freelancer-profiles/rating/{profile.Id}", new { rating = 5 }, _ct))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);

        var updated = await freelancer.GetFromJsonAsync<FreelancerProfileDto>("/freelancer-profiles/current", _ct);
        updated.Should().BeEquivalentTo(new { Rating = 5, Bio = "New" });
        updated!.Address.City.Should().Be("Iasi");
    }

    [Fact]
    public async Task List_IsAvailableToAnyAuthenticatedUser()
    {
        var profile = await UserProfilesApiFactory.CreateFreelancerProfileAsync(factory.CreateClient("Freelancer"), _ct);

        (await factory.CreateClient("Client").GetFromJsonAsync<List<FreelancerProfileDto>>("/freelancer-profiles", _ct))
            .Should().Contain(p => p.Id == profile.Id);
    }

    [Fact]
    public async Task Delete_RemovesProfile()
    {
        var freelancer = factory.CreateClient("Freelancer");
        var profile = await UserProfilesApiFactory.CreateFreelancerProfileAsync(freelancer, _ct);

        (await freelancer.DeleteAsync($"/freelancer-profiles/{profile.Id}", _ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await freelancer.GetAsync("/freelancer-profiles/current", _ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await freelancer.DeleteAsync($"/freelancer-profiles/{profile.Id}", _ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
