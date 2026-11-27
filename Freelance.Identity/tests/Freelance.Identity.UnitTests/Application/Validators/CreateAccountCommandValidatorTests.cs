using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Validators;

namespace Freelance.Identity.UnitTests.Application.Validators;

public sealed class CreateAccountCommandValidatorTests
{
    private static readonly CreateAccountCommand Valid = new("a@b.io", "secret", "0700000000", "alice", "Client");
    private readonly CreateAccountCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_Passes() => _validator.Validate(Valid).IsValid.Should().BeTrue();

    public static TheoryData<CreateAccountCommand, string> InvalidCommands => new()
    {
        { Valid with { Email = "" }, nameof(CreateAccountCommand.Email) },
        { Valid with { Email = "not-an-email" }, nameof(CreateAccountCommand.Email) },
        { Valid with { Password = "" }, nameof(CreateAccountCommand.Password) },
        { Valid with { Username = "" }, nameof(CreateAccountCommand.Username) },
        { Valid with { PhoneNumber = "" }, nameof(CreateAccountCommand.PhoneNumber) },
        { Valid with { PhoneNumber = "07000000001234" }, nameof(CreateAccountCommand.PhoneNumber) },
        { Valid with { Role = "" }, nameof(CreateAccountCommand.Role) },
        { Valid with { Role = "Admin" }, nameof(CreateAccountCommand.Role) }
    };

    [Theory]
    [MemberData(nameof(InvalidCommands))]
    public void Validate_InvalidField_ReportsThatField(CreateAccountCommand command, string property) =>
        _validator.Validate(command).Errors.Should().Contain(e => e.PropertyName == property);
}
