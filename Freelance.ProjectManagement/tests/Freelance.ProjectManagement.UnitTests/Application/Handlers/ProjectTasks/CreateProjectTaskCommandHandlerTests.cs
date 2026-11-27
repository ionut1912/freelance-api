using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;
using Freelance.ProjectManagement.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.ProjectTasks;

public sealed class CreateProjectTaskCommandHandlerTests : HandlerTestBase
{
    private CreateProjectTaskCommandHandler Handler =>
        new(ProjectRepository.Object, UnitOfWork.Object, NullLogger<CreateProjectTaskCommandHandler>.Instance);

    [Fact]
    public async Task Handle_MissingProject_ThrowsProjectNotFound()
    {
        GivenProject(null, tracked: true);

        await Handler.Invoking(h => h.Handle(new CreateProjectTaskCommand(Guid.NewGuid(), "t", "d", "New", "Low", TestData.Projects.ClientId), Ct))
            .Should().ThrowAsync<ProjectNotFoundException>();
        VerifyNotSaved();
    }

    [Fact]
    public async Task Handle_AddsTaskThroughTheAggregate_AndSaves()
    {
        var project = TestData.Projects.AssignedProject();
        GivenProject(project, tracked: true);

        var dto = await Handler.Handle(new CreateProjectTaskCommand(project.Id, "API", "Build", "InProgress", "Medium", TestData.Projects.ClientId), Ct);

        project.Tasks.Should().ContainSingle(t => t.Id == dto.Id);
        dto.Should().BeEquivalentTo(new { ProjectId = project.Id, Status = "InProgress", Priority = "Medium" });
        VerifySaved();
    }

    [Fact]
    public async Task Handle_NeitherOwnerNorAssignedFreelancer_ThrowsProjectNotFoundWithoutSaving()
    {
        var project = TestData.Projects.AssignedProject();
        GivenProject(project, tracked: true);

        await Handler.Invoking(h => h.Handle(new CreateProjectTaskCommand(project.Id, "API", "Build", "New", "Low", Guid.NewGuid()), Ct))
            .Should().ThrowAsync<ProjectNotFoundException>();
        VerifyNotSaved();
    }
}
