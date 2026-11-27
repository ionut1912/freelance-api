using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Application.Validators.TimeLogs;

namespace Freelance.ProjectManagement.UnitTests.Application.Validators;

public sealed class CreateTimeLogCommandValidatorTests
{
    private static readonly DateTime Start = TestData.Projects.Deadline;
    private readonly CreateTimeLogCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_Passes() =>
        _validator.Validate(new CreateTimeLogCommand(Guid.NewGuid(), Start, Start.AddHours(1))).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_EndBeforeStart_Fails() =>
        _validator.Validate(new CreateTimeLogCommand(Guid.NewGuid(), Start, Start.AddHours(-1))).Errors
            .Should().ContainSingle(e => e.PropertyName == "EndTime");

    [Fact]
    public void Validate_EmptyValues_ReportsTaskStartAndEnd() =>
        _validator.Validate(new CreateTimeLogCommand(Guid.Empty, default, default)).Errors.Select(e => e.PropertyName)
            .Should().Contain(["TaskId", "StartTime", "EndTime"]);
}
