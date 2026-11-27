using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Validators.FreelancerProfiles;

namespace Freelance.UserProfiles.UnitTests.Application.Validators;

public sealed class UpdateFreelancerDetailsCommandValidatorTests
{
    private static readonly UpdateFreelancerDetailsCommand Valid = new(Guid.NewGuid(), ["en"], ["C#"], ["Backend"], "5y", 10, "EUR", "p");
    private readonly UpdateFreelancerDetailsCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_Passes() => _validator.Validate(Valid).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_MismatchedSkillLists_Fails() =>
        _validator.Validate(Valid with { ProgrammingLanguages = ["C#", "Go"] }).IsValid.Should().BeFalse();

    [Fact]
    public void Validate_NonPositiveAmount_Fails() =>
        _validator.Validate(Valid with { Amount = -5 }).Errors.Should().Contain(e => e.PropertyName == "Amount");
}
