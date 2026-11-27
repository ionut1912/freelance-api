using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Application.Validators.TimeLogs;

namespace Freelance.ProjectManagement.UnitTests.Application.Validators;

public sealed class UpdateTimeLogCommandValidatorTests
{
    private static readonly DateTime Start = TestData.Projects.Deadline;
    private readonly UpdateTimeLogCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_Passes() =>
        _validator.Validate(new UpdateTimeLogCommand(Guid.NewGuid(), Guid.NewGuid(), Start, Start.AddHours(1))).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_MissingTaskAndInvertedInterval_Fails() =>
        _validator.Validate(new UpdateTimeLogCommand(Guid.NewGuid(), Guid.Empty, Start, Start)).Errors.Select(e => e.PropertyName)
            .Should().BeEquivalentTo("TaskId", "EndTime");
}
