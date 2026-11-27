using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Queries;
using Freelance.ProjectManagement.Domain.Exceptions;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.TimeLogs;

public sealed class GetTimeLogByIdQueryHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_Missing_ThrowsTimeLogNotFound()
    {
        GivenTimeLog(null);

        await new GetTimeLogByIdQueryHandler(TimeLogRepository.Object)
            .Invoking(h => h.Handle(new GetTimeLogByIdQuery(Guid.NewGuid()), Ct)).Should().ThrowAsync<TimeLogNotFoundException>();
    }

    [Fact]
    public async Task Handle_Existing_ReturnsDto()
    {
        var log = TestData.Projects.TimeLog();
        GivenTimeLog(log);

        (await new GetTimeLogByIdQueryHandler(TimeLogRepository.Object).Handle(new GetTimeLogByIdQuery(log.Id), Ct))
            .Id.Should().Be(log.Id);
    }
}
