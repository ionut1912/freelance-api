using Freelance.Identity.Application.Mediatr.Accounts.Query;
using Freelance.Identity.Application.Validators;

namespace Freelance.Identity.UnitTests.Application.Validators;

public sealed class GetCurrentAccountQueryValidatorTests
{
    private readonly GetCurrentAccountQueryValidator _validator = new();

    [Fact]
    public void Validate_WithUsername_Passes() =>
        _validator.Validate(new GetCurrentAccountQuery("alice")).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_EmptyUsername_Fails() =>
        _validator.Validate(new GetCurrentAccountQuery("")).IsValid.Should().BeFalse();
}
