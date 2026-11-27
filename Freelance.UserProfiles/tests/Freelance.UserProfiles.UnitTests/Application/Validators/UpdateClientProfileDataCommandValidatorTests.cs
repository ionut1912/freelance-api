using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Application.Validators.ClientProfiles;

namespace Freelance.UserProfiles.UnitTests.Application.Validators;

public sealed class UpdateClientProfileDataCommandValidatorTests
{
    private readonly UpdateClientProfileDataCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_Passes() =>
        _validator.Validate(new UpdateClientProfileDataCommand(Guid.NewGuid(), "b", "i")).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_EmptyValues_ReportsBioAndImage() =>
        _validator.Validate(new UpdateClientProfileDataCommand(Guid.NewGuid(), "", "")).Errors.Select(e => e.PropertyName)
            .Should().BeEquivalentTo("Bio", "Image");
}
