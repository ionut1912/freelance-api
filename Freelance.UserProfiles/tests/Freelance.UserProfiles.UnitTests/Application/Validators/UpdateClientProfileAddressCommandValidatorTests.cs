using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Application.Validators.ClientProfiles;
using Freelance.UserProfiles.UnitTests.TestData;

namespace Freelance.UserProfiles.UnitTests.Application.Validators;

public sealed class UpdateClientProfileAddressCommandValidatorTests
{
    private readonly UpdateClientProfileAddressCommandValidator _validator = new();

    [Fact]
    public void Validate_WithAddress_Passes() =>
        _validator.Validate(new UpdateClientProfileAddressCommand(Guid.NewGuid(), Profiles.Address)).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_WithoutAddress_Fails() =>
        _validator.Validate(new UpdateClientProfileAddressCommand(Guid.NewGuid(), null!)).IsValid.Should().BeFalse();
}
