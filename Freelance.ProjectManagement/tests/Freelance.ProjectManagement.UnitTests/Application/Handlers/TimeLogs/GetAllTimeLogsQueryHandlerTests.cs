using System.Linq.Expressions;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Handlers;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Queries;
using Freelance.ProjectManagement.Domain.Entities;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.TimeLogs;

public sealed class GetAllTimeLogsQueryHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsAllTimeLogsAsDtos()
    {
        TimeLogRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<TimeLog, object>>[]>()))
            .ReturnsAsync([TestData.Projects.TimeLog(), TestData.Projects.TimeLog()]);

        (await new GetAllTimeLogsQueryHandler(TimeLogRepository.Object).Handle(new GetAllTimeLogsQuery(), Ct))
            .Should().HaveCount(2);
    }
}
