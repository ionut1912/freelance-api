using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;
using Freelance.ProjectManagement.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.ProjectTasks;

public sealed class DeleteProjectTaskCommandHandlerTests : HandlerTestBase
{
    private DeleteProjectTaskCommandHandler Handler =>
        new(TaskRepository.Object, UnitOfWork.Object, NullLogger<DeleteProjectTaskCommandHandler>.Instance);

    [Fact]
    public async Task Handle_Missing_ThrowsProjectTaskNotFound()
    {
        GivenTask(null);

        await Handler.Invoking(h => h.Handle(new DeleteProjectTaskCommand(Guid.NewGuid()), Ct))
            .Should().ThrowAsync<ProjectTaskNotFoundException>();
    }

    [Fact]
    public async Task Handle_Existing_DeletesAndSaves()
    {
        var task = TestData.Projects.Task();
        GivenTask(task);

        await Handler.Handle(new DeleteProjectTaskCommand(task.Id), Ct);

        TaskRepository.Verify(r => r.Delete(task), Times.Once);
        VerifySaved();
    }
}
