using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;

namespace Freelance.UserProfiles.UnitTests.Domain.Entities;

/// <summary>Behaviour both profile types inherit.</summary>
public sealed class BaseUserProfileTests
{
    private static BaseUserProfile Profile(string kind) =>
        kind == "Client" ? Profiles.Client() : Profiles.Freelancer();

    [Theory]
    [InlineData("Client")]
    [InlineData("Freelancer")]
    public void Verify_CalledTwice_StaysVerified(string kind)
    {
        var profile = Profile(kind);

        profile.Verify();
        profile.Verify();

        profile.IsVerified.Should().BeTrue();
    }

    [Theory]
    [InlineData("Client")]
    [InlineData("Freelancer")]
    public void UpdateAddress_KeepsVerification(string kind)
    {
        var profile = Profile(kind);
        profile.Verify();

        profile.UpdateAddress("s", "c", "st", "z", "co", "n");

        profile.IsVerified.Should().BeTrue();
    }

    [Theory]
    [InlineData("Client")]
    [InlineData("Freelancer")]
    public void UpdateUserData_Rejected_KeepsVerificationAndData(string kind)
    {
        var profile = Profile(kind);
        profile.Verify();

        profile.Invoking(p => p.UpdateUserData("new-image", "Bio")).Should().Throw<BioAlreadyExistsException>();

        profile.Should().BeEquivalentTo(new { IsVerified = true, Image = "img", Bio = "Bio" },
            options => options.ExcludingMissingMembers());
    }
}
