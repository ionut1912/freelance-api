using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Application.Validators.Projects;

namespace Freelance.ProjectManagement.UnitTests.Application.Validators;

public sealed class UpdateProjectCommandValidatorTests
{
    private static readonly UpdateProjectCommand Valid = new(Guid.NewGuid(), "", "", TestData.Projects.Deadline, 10, "EUR", []);
    private readonly UpdateProjectCommandValidator _validator = new();

    [Fact]
    public void Validate_PartialUpdate_Passes() => _validator.Validate(Valid).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_InvalidAmountAndCurrency_Fails() =>
        _validator.Validate(Valid with { Amount = -1, Currency = "EURO" }).Errors.Select(e => e.PropertyName)
            .Should().BeEquivalentTo(nameof(UpdateProjectCommand.Amount), nameof(UpdateProjectCommand.Currency));
}
