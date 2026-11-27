using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;
using Freelance.ProjectManagement.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.TimeLogs;

public sealed class DeleteTimeLogCommandHandlerTests : HandlerTestBase
{
    private DeleteTimeLogCommandHandler Handler =>
        new(TimeLogRepository.Object, UnitOfWork.Object, NullLogger<DeleteTimeLogCommandHandler>.Instance);

    [Fact]
    public async Task Handle_Missing_ThrowsTimeLogNotFound()
    {
        GivenTimeLog(null);

        await Handler.Invoking(h => h.Handle(new DeleteTimeLogCommand(Guid.NewGuid()), Ct)).Should().ThrowAsync<TimeLogNotFoundException>();
    }

    [Fact]
    public async Task Handle_Existing_DeletesAndSaves()
    {
        var log = TestData.Projects.TimeLog();
        GivenTimeLog(log);

        await Handler.Handle(new DeleteTimeLogCommand(log.Id), Ct);

        TimeLogRepository.Verify(r => r.Delete(log), Times.Once);
        VerifySaved();
    }
}
