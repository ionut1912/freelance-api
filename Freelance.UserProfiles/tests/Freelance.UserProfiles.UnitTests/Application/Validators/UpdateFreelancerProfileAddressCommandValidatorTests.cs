using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Validators.FreelancerProfiles;
using Freelance.UserProfiles.UnitTests.TestData;

namespace Freelance.UserProfiles.UnitTests.Application.Validators;

public sealed class UpdateFreelancerProfileAddressCommandValidatorTests
{
    private readonly UpdateFreelancerProfileAddressCommandValidator _validator = new();

    [Fact]
    public void Validate_WithAddress_Passes() =>
        _validator.Validate(new UpdateFreelancerProfileAddressCommand(Guid.NewGuid(), Profiles.Address, Guid.NewGuid())).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_WithoutAddress_Fails() =>
        _validator.Validate(new UpdateFreelancerProfileAddressCommand(Guid.NewGuid(), null!, Guid.NewGuid())).IsValid.Should().BeFalse();
}
