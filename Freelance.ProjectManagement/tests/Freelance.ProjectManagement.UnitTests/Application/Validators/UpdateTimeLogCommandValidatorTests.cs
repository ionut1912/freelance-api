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

    /// <summary>Raw clock values would say 11:30 is after 10:00, but 11:30+02:00 is 09:30 UTC.</summary>
    [Fact]
    public void Validate_EndThatIsEarlierOnceConvertedToUtc_Fails()
    {
        var start = new DateTime(2030, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var end = new DateTimeOffset(2030, 1, 1, 11, 30, 0, TimeSpan.FromHours(2)).LocalDateTime;

        _validator.Validate(new UpdateTimeLogCommand(Guid.NewGuid(), Guid.NewGuid(), start, end)).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_MissingStartTime_Fails() =>
        _validator.Validate(new UpdateTimeLogCommand(Guid.NewGuid(), Guid.NewGuid(), default, new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc)))
            .Errors.Should().Contain(e => e.PropertyName == "StartTime");
}
