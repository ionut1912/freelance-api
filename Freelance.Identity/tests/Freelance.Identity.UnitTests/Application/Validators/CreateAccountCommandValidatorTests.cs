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

    [Theory]
    [InlineData(50, 'a', 72, true)]
    [InlineData(51, 'a', 72, false)]
    [InlineData(50, 'a', 73, false)]
    [InlineData(50, 'é', 36, true)]
    [InlineData(50, 'é', 37, false)]
    public void Validate_UsernameAndPasswordLimits(int usernameLength, char passwordCharacter, int passwordLength, bool valid) =>
        new CreateAccountCommandValidator()
            .Validate(new CreateAccountCommand("a@test.io", new string(passwordCharacter, passwordLength), "0700000000",
                new string('u', usernameLength), "Client"))
            .IsValid.Should().Be(valid);

    [Fact]
    public void Validate_NullPassword_ReportsRequiredWithoutThrowing() =>
        new CreateAccountCommandValidator()
            .Validate(new CreateAccountCommand("a@test.io", null!, "0700000000", "user", "Client"))
            .Errors.Should().ContainSingle(e => e.PropertyName == nameof(CreateAccountCommand.Password));
}
