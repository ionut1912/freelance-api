using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Application.Validators.Projects;

namespace Freelance.ProjectManagement.UnitTests.Application.Validators;

public sealed class UpdateProjectCommandValidatorTests
{
    private static readonly UpdateProjectCommand Valid = new(Guid.NewGuid(), "", "", TestData.Projects.Deadline, 10, "EUR", [], Guid.NewGuid());
    private readonly UpdateProjectCommandValidator _validator = new();

    [Fact]
    public void Validate_PartialUpdate_Passes() => _validator.Validate(Valid).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_InvalidAmountAndCurrency_Fails() =>
        _validator.Validate(Valid with { Amount = -1, Currency = "EURO" }).Errors.Select(e => e.PropertyName)
            .Should().BeEquivalentTo(nameof(UpdateProjectCommand.Amount), nameof(UpdateProjectCommand.Currency));

    [Fact]
    public void Validate_MissingDeadlineAndCurrency_Fails() =>
        _validator.Validate(Valid with { Deadline = default, Currency = null! }).Errors.Select(e => e.PropertyName).Distinct()
            .Should().BeEquivalentTo(nameof(UpdateProjectCommand.Deadline), nameof(UpdateProjectCommand.Currency));

    [Fact]
    public void Validate_TextLongerThanColumns_Fails() =>
        _validator.Validate(Valid with { Title = new string('a', 51), Description = new string('a', 51) }).Errors
            .Select(e => e.PropertyName)
            .Should().BeEquivalentTo(nameof(UpdateProjectCommand.Title), nameof(UpdateProjectCommand.Description));
}
