using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Validators.FreelancerProfiles;
using Freelance.UserProfiles.UnitTests.TestData;

namespace Freelance.UserProfiles.UnitTests.Application.Validators;

public sealed class CreateFreelancerProfileCommandValidatorTests
{
    private static readonly CreateFreelancerProfileCommand Valid =
        new(Guid.NewGuid(), Profiles.Address, "b", "i", "5y", 10, "EUR", "p", ["en"], ["C#"], ["Backend"]);
    private readonly CreateFreelancerProfileCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_Passes() => _validator.Validate(Valid).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_MismatchedSkillLists_Fails() =>
        _validator.Validate(Valid with { Areas = ["Backend", "Cloud"] }).Errors
            .Should().Contain(e => e.ErrorMessage.Contains("same number", StringComparison.Ordinal));

    [Fact]
    public void Validate_EmptyCommand_ReportsRequiredFields() =>
        _validator.Validate(new CreateFreelancerProfileCommand(Guid.Empty, null!, "", "", "", 0, "", "", [], [], [])).Errors
            .Select(e => e.PropertyName).Should().Contain(["AccountId", "Address", "Bio", "Image", "Experience", "Amount", "Currency",
                "PortfolioUrl", "ForeignLanguages", "ProgrammingLanguages", "Areas"]);

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void Validate_OneSkillListMissing_ReportsTheMismatch(bool noLanguages, bool noAreas) =>
        _validator.Validate(Valid with
        {
            ProgrammingLanguages = noLanguages ? null! : ["C#"],
            Areas = noAreas ? null! : ["Backend"]
        }).Errors.Should().Contain(e => e.ErrorMessage.Contains("same number", StringComparison.Ordinal));
}
