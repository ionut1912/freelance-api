using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;
using Freelance.ProjectManagement.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.ProjectTasks;

public sealed class DeleteProjectTaskCommandHandlerTests : HandlerTestBase
{
    private DeleteProjectTaskCommandHandler Handler =>
        new(TaskRepository.Object, ProjectRepository.Object, UnitOfWork.Object, NullLogger<DeleteProjectTaskCommandHandler>.Instance);

    [Fact]
    public async Task Handle_Missing_ThrowsProjectTaskNotFound()
    {
        GivenTask(null);

        await Handler.Invoking(h => h.Handle(new DeleteProjectTaskCommand(Guid.NewGuid(), Guid.Empty), Ct))
            .Should().ThrowAsync<ProjectTaskNotFoundException>();
    }

    [Fact]
    public async Task Handle_Existing_DeletesAndSaves()
    {
        var project = TestData.Projects.Project();
        var task = TestData.Projects.Task(project.Id);
        GivenTask(task);
        GivenProject(project);

        await Handler.Handle(new DeleteProjectTaskCommand(task.Id, project.ClientId), Ct);

        TaskRepository.Verify(r => r.Delete(task), Times.Once);
        VerifySaved();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_ProjectMissingOrOfAnotherClient_ThrowsWithoutDeleting(bool projectExists)
    {
        var task = TestData.Projects.Task();
        GivenTask(task);
        GivenProject(projectExists ? TestData.Projects.Project() : null);

        await Handler.Invoking(h => h.Handle(new DeleteProjectTaskCommand(task.Id, Guid.NewGuid()), Ct))
            .Should().ThrowAsync<ProjectTaskNotFoundException>();
        VerifyNotSaved();
    }
}
