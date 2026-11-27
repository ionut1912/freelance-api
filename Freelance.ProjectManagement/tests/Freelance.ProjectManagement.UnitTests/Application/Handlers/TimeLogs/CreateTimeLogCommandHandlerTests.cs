using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;
using Freelance.ProjectManagement.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.TimeLogs;

public sealed class CreateTimeLogCommandHandlerTests : HandlerTestBase
{
    private CreateTimeLogCommandHandler Handler =>
        new(TaskRepository.Object, UnitOfWork.Object, NullLogger<CreateTimeLogCommandHandler>.Instance);

    [Fact]
    public async Task Handle_MissingTask_ThrowsProjectTaskNotFound()
    {
        GivenTask(null, tracked: true);

        await Handler.Invoking(h => h.Handle(new CreateTimeLogCommand(Guid.NewGuid(), TestData.Projects.Deadline, TestData.Projects.Deadline), Ct))
            .Should().ThrowAsync<ProjectTaskNotFoundException>();
    }

    [Fact]
    public async Task Handle_AddsTimeLogToTask_AndSaves()
    {
        var task = TestData.Projects.Task();
        GivenTask(task, tracked: true);

        var dto = await Handler.Handle(new CreateTimeLogCommand(task.Id, TestData.Projects.Deadline, TestData.Projects.Deadline.AddHours(2)), Ct);

        task.TimeLogs.Should().ContainSingle(l => l.Id == dto.Id);
        dto.TotalHours.Should().Be(2);
        VerifySaved();
    }
}
