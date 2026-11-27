using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Application.Validators.ClientProfiles;
using Freelance.UserProfiles.UnitTests.TestData;

namespace Freelance.UserProfiles.UnitTests.Application.Validators;

public sealed class CreateClientProfileCommandValidatorTests
{
    private readonly CreateClientProfileCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_Passes() =>
        _validator.Validate(new CreateClientProfileCommand(Guid.NewGuid(), Profiles.Address, "b", "i")).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_EmptyCommand_ReportsEveryField() =>
        _validator.Validate(new CreateClientProfileCommand(Guid.Empty, null!, "", "")).Errors.Select(e => e.PropertyName)
            .Should().BeEquivalentTo("AccountId", "AddressDto", "Bio", "Image");
}
