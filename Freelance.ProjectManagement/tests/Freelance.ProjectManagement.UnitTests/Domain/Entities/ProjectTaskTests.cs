using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Freelance.ProjectManagement.UnitTests.TestData;

namespace Freelance.ProjectManagement.UnitTests.Domain.Entities;

public sealed class ProjectTaskTests
{
    [Fact]
    public void Create_RequiresStatusAndPriority()
    {
        FluentActions.Invoking(() => ProjectTask.Create(Guid.NewGuid(), "t", "d", null!, ProjectTaskPriority.Low))
            .Should().Throw<ArgumentNullException>();
        FluentActions.Invoking(() => ProjectTask.Create(Guid.NewGuid(), "t", "d", ProjectTaskStatus.New, null!))
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Update_KeepsEmptyValues_AndSetsStatusPriority()
    {
        var task = Projects.Task();

        task.Update("", "", ProjectTaskStatus.Review, ProjectTaskPriority.Medium);

        task.Should().BeEquivalentTo(new
        {
            Title = "API",
            Description = "Build API",
            Status = ProjectTaskStatus.Review,
            Priority = ProjectTaskPriority.Medium
        });
    }

    [Fact]
    public void AddTimeLog_ThenUpdateTimeLog_ChangesInterval()
    {
        var task = Projects.Task();
        var log = Projects.TimeLog(task.Id);
        task.AddTimeLog(log);

        task.UpdateTimeLog(log.Id, Projects.Deadline, Projects.Deadline.AddHours(5));

        task.TimeLogs.Should().ContainSingle().Which.TotalHours.Should().Be(5);
    }

    [Fact]
    public void UpdateTimeLog_Unknown_Throws() =>
        Projects.Task().Invoking(t => t.UpdateTimeLog(Guid.NewGuid(), Projects.Deadline, Projects.Deadline))
            .Should().Throw<TimeLogNotFoundException>();

    [Fact]
    public void AddTimeLog_Null_Throws() => Projects.Task().Invoking(t => t.AddTimeLog(null!)).Should().Throw<ArgumentNullException>();

    [Fact]
    public void AssignFreelancer_SetsFreelancer()
    {
        var task = Projects.Task();
        var freelancer = Guid.NewGuid();

        task.AssignFreelancer(freelancer);

        task.FreelacerId.Should().Be(freelancer);
    }
}
