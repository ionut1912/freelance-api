using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Requests;
using Freelance.ProjectManagement.UnitTests.TestData;

namespace Freelance.ProjectManagement.UnitTests.Application.Mappings;

public sealed class TimeLogMappingTests
{
    [Fact]
    public void ToDto_MapsAllFields()
    {
        var log = Projects.TimeLog();

        log.ToDto().Should().BeEquivalentTo(new { log.Id, log.TaskId, log.StartTime, log.EndTime, TotalHours = 3 });
        new[] { log }.ToDtos().Should().ContainSingle();
    }

    [Fact]
    public void Requests_MapToCommands()
    {
        var taskId = Guid.NewGuid();
        var id = Guid.NewGuid();

        new CreateTimeLogRequest(taskId, Projects.Deadline, Projects.Deadline.AddHours(1)).ToCreateCommand(Guid.Empty)
            .TaskId.Should().Be(taskId);
        new UpdateTimeLogRequest(taskId, Projects.Deadline, Projects.Deadline.AddHours(1)).ToUpdateCommand(id, Guid.Empty)
            .Should().BeEquivalentTo(new { Id = id, TaskId = taskId });
    }
}
