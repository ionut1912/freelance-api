using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Requests;
using Freelance.ProjectManagement.UnitTests.TestData;

namespace Freelance.ProjectManagement.UnitTests.Application.Mappings;

public sealed class TaskMappingTests
{
    [Fact]
    public void ToDto_MapsTaskWithTimeLogs()
    {
        var task = Projects.Task();
        task.AddTimeLog(Projects.TimeLog(task.Id));

        var dto = task.ToDto();

        dto.Should().BeEquivalentTo(new { task.Id, task.ProjectId, Title = "API", Status = "New", Priority = "High" });
        dto.TimeLogs.Should().ContainSingle(l => l.TotalHours == 3);
        new[] { task }.ToDtos().Should().ContainSingle();
    }

    [Fact]
    public void Requests_MapToCommands()
    {
        var projectId = Guid.NewGuid();
        var id = Guid.NewGuid();

        new CreateProjectTaskRequest(projectId, "t", "d", "New", "Low").ToCreateCommand()
            .Should().BeEquivalentTo(new { ProjectId = projectId, Title = "t", Status = "New", Priority = "Low" });
        new UpdateProjectTaskReuqest(projectId, "t", "d", "Done", "High").ToUpdateCommand(id)
            .Should().BeEquivalentTo(new { Id = id, ProjectId = projectId, Status = "Done" });
    }
}
