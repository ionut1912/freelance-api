using Freelance.Identity.Application.Mediatr.Accounts.Query;
using Freelance.Identity.Application.Validators;

namespace Freelance.Identity.UnitTests.Application.Validators;

public sealed class LoginQueryValidatorTests
{
    private readonly LoginQueryValidator _validator = new();

    [Fact]
    public void Validate_WithCredentials_Passes() =>
        _validator.Validate(new LoginQuery("alice", "secret")).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_EmptyCredentials_ReportsBothFields() =>
        _validator.Validate(new LoginQuery("", "")).Errors.Select(e => e.PropertyName)
            .Should().BeEquivalentTo(nameof(LoginQuery.Username), nameof(LoginQuery.Password));
}
