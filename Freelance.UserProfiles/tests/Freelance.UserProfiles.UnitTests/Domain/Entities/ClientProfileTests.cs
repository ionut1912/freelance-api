using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.ValueObjects;
using Freelance.UserProfiles.UnitTests.TestData;

namespace Freelance.UserProfiles.UnitTests.Domain.Entities;

/// <summary>Covers <see cref="ClientProfile"/> and the behaviour it inherits from <see cref="BaseUserProfile"/>.</summary>
public sealed class ClientProfileTests
{
    [Fact]
    public void Create_SetsValues_AndStartsUnverified()
    {
        var accountId = Guid.NewGuid();

        var profile = Profiles.Client(accountId);

        profile.Should().BeEquivalentTo(new { AccountId = accountId, Bio = "Bio", Image = "img", IsVerified = false });
        profile.Address.Should().Be(Address.Create("Main", "Bucharest", "B", "010101", "RO", "10"));
    }

    [Fact]
    public void Verify_MarksVerified()
    {
        var profile = Profiles.Client();

        profile.Verify();

        profile.IsVerified.Should().BeTrue();
        profile.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void UpdateAddress_ReplacesAddress()
    {
        var profile = Profiles.Client();

        profile.UpdateAddress("Second", "Cluj", "CJ", "400000", "RO", "2");

        profile.Address.City.Should().Be("Cluj");
    }

    [Fact]
    public void UpdateUserData_ChangesBioAndImage()
    {
        var profile = Profiles.Client();

        profile.UpdateUserData("new-img", "New bio");

        profile.Image.Should().Be("new-img");
        profile.Bio.Should().Be("New bio");
    }

    [Fact]
    public void UpdateUserData_SameImage_Throws() =>
        Profiles.Client().Invoking(p => p.UpdateUserData("img", "Other")).Should().Throw<ImageAlreadyExistsException>();

    [Fact]
    public void UpdateUserData_SameBio_Throws() =>
        Profiles.Client().Invoking(p => p.UpdateUserData("other", "Bio")).Should().Throw<BioAlreadyExistsException>();
}
