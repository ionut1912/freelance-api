using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Validators.FreelancerProfiles;

namespace Freelance.UserProfiles.UnitTests.Application.Validators;

public sealed class UpdateProfileDataCommandValidatorTests
{
    private readonly UpdateProfileDataCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_Passes() =>
        _validator.Validate(new UpdateFreelancerProfileDataCommand(Guid.NewGuid(), "b", "i", Guid.NewGuid())).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_EmptyValues_ReportsBioAndImage() =>
        _validator.Validate(new UpdateFreelancerProfileDataCommand(Guid.NewGuid(), "", "", Guid.NewGuid())).Errors.Select(e => e.PropertyName)
            .Should().BeEquivalentTo("Bio", "Image");
}
