using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Application.Validators.ProjectTask;

namespace Freelance.ProjectManagement.UnitTests.Application.Validators;

public sealed class CreateProjectTaskCommandValidatorTests
{
    private static readonly CreateProjectTaskCommand Valid = new(Guid.NewGuid(), "API", "Build", "New", "High");
    private readonly CreateProjectTaskCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_Passes() => _validator.Validate(Valid).IsValid.Should().BeTrue();

    public static TheoryData<CreateProjectTaskCommand, string> Invalid => new()
    {
        { Valid with { ProjectId = Guid.Empty }, nameof(CreateProjectTaskCommand.ProjectId) },
        { Valid with { Title = "" }, nameof(CreateProjectTaskCommand.Title) },
        { Valid with { Description = "" }, nameof(CreateProjectTaskCommand.Description) },
        { Valid with { Status = "Bogus" }, nameof(CreateProjectTaskCommand.Status) },
        { Valid with { Priority = "Urgent" }, nameof(CreateProjectTaskCommand.Priority) }
    };

    [Theory]
    [MemberData(nameof(Invalid))]
    public void Validate_InvalidField_ReportsIt(CreateProjectTaskCommand command, string property) =>
        _validator.Validate(command).Errors.Should().Contain(e => e.PropertyName == property);

    [Fact]
    public void Validate_InvalidStatus_ListsAllowedValues() =>
        _validator.Validate(Valid with { Status = "Bogus" }).Errors.Should()
            .Contain(e => e.ErrorMessage.Contains("'InProgress'", StringComparison.Ordinal));
}
