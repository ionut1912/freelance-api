using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;
using Freelance.ProjectManagement.Domain.Exceptions;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.ProjectTasks;

public sealed class UpdateProjectTaskCommandHandlerTests : HandlerTestBase
{
    private UpdateProjectTaskCommandHandler Handler =>
        new(ProjectRepository.Object, UnitOfWork.Object, NullLogger<UpdateProjectTaskCommandHandler>.Instance);

    [Fact]
    public async Task Handle_MissingProject_ThrowsProjectNotFound()
    {
        GivenProject(null, tracked: true);

        await Handler.Invoking(h => h.Handle(new UpdateProjectTaskCommand(Guid.NewGuid(), Guid.NewGuid(), "t", "d", "Done", "Low"), Ct))
            .Should().ThrowAsync<ProjectNotFoundException>();
    }

    [Fact]
    public async Task Handle_TaskNotInProject_ThrowsProjectTaskNotFound()
    {
        GivenProject(TestData.Projects.Project(), tracked: true);

        await Handler.Invoking(h => h.Handle(new UpdateProjectTaskCommand(Guid.NewGuid(), Guid.NewGuid(), "t", "d", "Done", "Low"), Ct))
            .Should().ThrowAsync<ProjectTaskNotFoundException>();
        VerifyNotSaved();
    }

    [Fact]
    public async Task Handle_UpdatesTaskAndSaves()
    {
        var project = TestData.Projects.Project();
        var task = TestData.Projects.Task(project.Id);
        project.AddTask(task);
        GivenProject(project, tracked: true);

        await Handler.Handle(new UpdateProjectTaskCommand(task.Id, project.Id, "v2", "d2", "Done", "Low"), Ct);

        task.Status.Should().Be(ProjectTaskStatus.Done);
        task.Title.Should().Be("v2");
        VerifySaved();
    }

    [Fact]
    public async Task Handle_ProjectOfAnotherClient_ThrowsProjectNotFoundWithoutSaving()
    {
        var project = TestData.Projects.Project();
        GivenProject(project, tracked: true);

        await Handler.Invoking(h => h.Handle(new UpdateProjectTaskCommand(Guid.NewGuid(), project.Id, "v2", "d2", "Done", "Low", Guid.NewGuid()), Ct))
            .Should().ThrowAsync<ProjectNotFoundException>();
        VerifyNotSaved();
    }
}
