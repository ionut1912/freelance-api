using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Requests;
using Freelance.UserProfiles.UnitTests.TestData;

namespace Freelance.UserProfiles.UnitTests.Application.Mappings;

public sealed class FreelancerProfileMappingsTests
{
    [Fact]
    public void ToDto_FlattensLanguagesAndSkills()
    {
        var profile = Profiles.Freelancer();

        var dto = profile.ToDto();

        dto.Should().BeEquivalentTo(new
        {
            profile.Id,
            Amount = 50m,
            Currency = "EUR",
            ForeignLanguages = new[] { "English" },
            ProgrammingLanguages = new[] { "C#" },
            Areas = new[] { "Backend" }
        });
        new[] { profile }.ToDtos().Should().ContainSingle();
    }

    [Fact]
    public void Requests_MapToCommands()
    {
        var id = Guid.NewGuid();

        new CreateFreelancerProfileRequest(Profiles.Address, "b", "i", "e", 1, "EUR", "p", ["en"], ["C#"], ["Backend"]).ToCreateCommand(id)
            .Should().BeEquivalentTo(new { AccountId = id, Experience = "e", ForeignLanguages = new[] { "en" }, Areas = new[] { "Backend" } });
        new UpdateFreelancerDetailRequest(["en"], ["C#"], ["Backend"], "e", 2, "USD", "p").ToUpdateDetailsCommand(id)
            .Should().BeEquivalentTo(new { Id = id, Amount = 2m, Currency = "USD" });
        new UpdateFreelancerProfileRatingRequest(5).ToUpdateRatingCommand(id).Should().BeEquivalentTo(new { Id = id, Rating = 5 });
        new UpdateProfileAddressRequest(Profiles.Address).ToUpdateFreelancerAddressCommand(id).Id.Should().Be(id);
        new UpdateProfileDataRequest("b", "i").ToUpdateFreelancerDataCommand(id).Bio.Should().Be("b");
        new VerifyProfileRequest("url").ToVerifyFreelancerCommand(id).ImageUrl.Should().Be("url");
    }
}
