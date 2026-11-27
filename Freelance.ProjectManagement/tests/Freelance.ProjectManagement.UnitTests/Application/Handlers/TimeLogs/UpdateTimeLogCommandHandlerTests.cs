using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;
using Freelance.ProjectManagement.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.TimeLogs;

public sealed class UpdateTimeLogCommandHandlerTests : HandlerTestBase
{
    private UpdateTimeLogCommandHandler Handler =>
        new(TaskRepository.Object, UnitOfWork.Object, NullLogger<UpdateTimeLogCommandHandler>.Instance);

    [Fact]
    public async Task Handle_MissingTask_ThrowsProjectTaskNotFound()
    {
        GivenTask(null, tracked: true);

        await Handler.Invoking(h => h.Handle(new UpdateTimeLogCommand(Guid.NewGuid(), Guid.NewGuid(), TestData.Projects.Deadline, TestData.Projects.Deadline), Ct))
            .Should().ThrowAsync<ProjectTaskNotFoundException>();
    }

    [Fact]
    public async Task Handle_TimeLogNotInTask_ThrowsTimeLogNotFound()
    {
        GivenTask(TestData.Projects.Task(), tracked: true);

        await Handler.Invoking(h => h.Handle(new UpdateTimeLogCommand(Guid.NewGuid(), Guid.NewGuid(), TestData.Projects.Deadline, TestData.Projects.Deadline), Ct))
            .Should().ThrowAsync<TimeLogNotFoundException>();
        VerifyNotSaved();
    }

    [Fact]
    public async Task Handle_UpdatesIntervalAndSaves()
    {
        var task = TestData.Projects.Task();
        var log = TestData.Projects.TimeLog(task.Id);
        task.AddTimeLog(log);
        GivenTask(task, tracked: true);

        await Handler.Handle(new UpdateTimeLogCommand(log.Id, task.Id, TestData.Projects.Deadline, TestData.Projects.Deadline.AddHours(6)), Ct);

        log.TotalHours.Should().Be(6);
        VerifySaved();
    }
}
