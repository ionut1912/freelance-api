using Freelance.ProjectManagement.Application.Mappings;
using Freelance.ProjectManagement.Application.Requests;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Freelance.ProjectManagement.UnitTests.TestData;

namespace Freelance.ProjectManagement.UnitTests.Application.Mappings;

public sealed class ProjectMappingsTests
{
    [Fact]
    public void ToDto_MapsProjectWithTechnologiesAndTasks()
    {
        var project = Projects.Project();
        project.AddTechnologies([ProjectTechnology.Create("dotnet")]);
        project.AddTask(Projects.Task(project.Id));

        var dto = project.ToDto();

        dto.Should().BeEquivalentTo(new { project.Id, project.Title, project.Description, Amount = 1000m, Currency = "EUR" });
        dto.Technologies.Should().ContainSingle(t => t.Technology == "dotnet");
        dto.ProjectTasks.Should().ContainSingle(t => t.Title == "API");
        new[] { project }.ToDtos().Should().ContainSingle();
    }

    [Fact]
    public void Requests_MapToCommands()
    {
        var create = new CreateProjectRequest("t", "d", Projects.Deadline, 5, "EUR", ["x"]).ToCreateCommand(Guid.Empty);
        var id = Guid.NewGuid();
        var update = new UpdateProjectRequest("t2", "d2", Projects.Deadline, 6, "USD", ["y"]).ToUpdateCommand(id, id);

        create.Should().BeEquivalentTo(new { Title = "t", Description = "d", Amount = 5m, Currency = "EUR", Technologies = new[] { "x" } });
        update.Should().BeEquivalentTo(new { Id = id, ClientId = id, Title = "t2", Amount = 6m, Technologies = new[] { "y" } });
    }
}
