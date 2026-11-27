using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.UnitTests.TestData;

namespace Freelance.ProjectManagement.UnitTests.Domain.Entities;

public sealed class TimeLogTests
{
    [Fact]
    public void Create_AcrossMidnight_ComputesWholeHours()
    {
        var log = TimeLog.Create(Guid.NewGuid(), new DateTime(2026, 11, 26, 22, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 11, 27, 1, 30, 0, DateTimeKind.Utc));

        log.TotalHours.Should().Be(3);
    }

    [Fact]
    public void Create_EndBeforeStart_Throws() =>
        FluentActions.Invoking(() => TimeLog.Create(Guid.NewGuid(), Projects.Deadline, Projects.Deadline.AddMinutes(-1)))
            .Should().Throw<ArgumentException>();

    [Fact]
    public void Create_NormalizesUnspecifiedKindToUtc()
    {
        var log = TimeLog.Create(Guid.NewGuid(), new DateTime(2026, 1, 1, 8, 0, 0), new DateTime(2026, 1, 1, 9, 0, 0));

        log.StartTime.Kind.Should().Be(DateTimeKind.Utc);
        log.EndTime.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void Update_RecomputesTotalHours_AndTimestamp()
    {
        var log = Projects.TimeLog();

        log.Update(Projects.Deadline, Projects.Deadline.AddHours(8));

        log.TotalHours.Should().Be(8);
        log.UpdatedAt.Should().NotBeNull();
    }
}
