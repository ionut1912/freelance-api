using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Validators;

namespace Freelance.Identity.UnitTests.Application.Validators;

public sealed class BlockAccountCommandValidatorTests
{
    private readonly BlockAccountCommandValidator _validator = new();

    [Fact]
    public void Validate_WithAccountId_Passes() =>
        _validator.Validate(new BlockAccountCommand(Guid.NewGuid())).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_EmptyAccountId_Fails() =>
        _validator.Validate(new BlockAccountCommand(Guid.Empty)).Errors.Should().ContainSingle(e => e.PropertyName == "AccountId");
}
