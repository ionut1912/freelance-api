using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Validators.FreelancerProfiles;

namespace Freelance.UserProfiles.UnitTests.Application.Validators;

public sealed class UpdateRatingCommandValidatorTests
{
    private readonly UpdateRatingCommandValidator _validator = new();

    [Fact]
    public void Validate_WithRating_Passes() =>
        _validator.Validate(new UpdateFreelancerRatingCommand(Guid.NewGuid(), 4)).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_ZeroRating_Fails() =>
        _validator.Validate(new UpdateFreelancerRatingCommand(Guid.NewGuid(), 0)).IsValid.Should().BeFalse();
}
