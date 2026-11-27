using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Application.Validators.Projects;

namespace Freelance.ProjectManagement.UnitTests.Application.Validators;

public sealed class CreateProjectCommandValidatorTests
{
    private static readonly CreateProjectCommand Valid = new("Site", "Build", TestData.Projects.Deadline, 100, "EUR", ["dotnet"]);
    private readonly CreateProjectCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_Passes() => _validator.Validate(Valid).IsValid.Should().BeTrue();

    public static TheoryData<CreateProjectCommand, string> Invalid => new()
    {
        { Valid with { Title = "" }, nameof(CreateProjectCommand.Title) },
        { Valid with { Description = "" }, nameof(CreateProjectCommand.Description) },
        { Valid with { Deadline = default }, nameof(CreateProjectCommand.Deadline) },
        { Valid with { Amount = 0 }, nameof(CreateProjectCommand.Amount) },
        { Valid with { Currency = "EU" }, nameof(CreateProjectCommand.Currency) },
        { Valid with { Technologies = [] }, nameof(CreateProjectCommand.Technologies) }
    };

    [Theory]
    [MemberData(nameof(Invalid))]
    public void Validate_InvalidField_ReportsIt(CreateProjectCommand command, string property) =>
        _validator.Validate(command).Errors.Should().Contain(e => e.PropertyName == property);
}
