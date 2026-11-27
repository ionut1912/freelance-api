using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Requests;
using Freelance.UserProfiles.UnitTests.TestData;

namespace Freelance.UserProfiles.UnitTests.Application.Mappings;

public sealed class ClientProfileMappingsTests
{
    [Fact]
    public void ToDto_MapsProfile()
    {
        var profile = Profiles.Client();

        profile.ToDto().Should().BeEquivalentTo(new { profile.Id, profile.AccountId, Bio = "Bio", Image = "img", IsVerified = false, Address = Profiles.Address });
        new[] { profile }.ToDtos().Should().ContainSingle();
    }

    [Fact]
    public void Requests_MapToCommands()
    {
        var id = Guid.NewGuid();

        new CreateClientProfileRequest(Profiles.Address, "b", "i").ToCreateCommand(id)
            .Should().BeEquivalentTo(new { AccountId = id, AddressDto = Profiles.Address, Bio = "b", Image = "i" });
        new UpdateProfileAddressRequest(Profiles.Address).ToUpdateClientAddressCommand(id, id).AccountId.Should().Be(id);
        new UpdateProfileDataRequest("b", "i").ToUpdateClientDataCommand(id, id).Should().BeEquivalentTo(new { Id = id, AccountId = id, Bio = "b", Image = "i" });
        new VerifyProfileRequest("url").ToVerifyClientCommand(id).Should().BeEquivalentTo(new { AccountId = id, ImageUrl = "url" });
    }
}
