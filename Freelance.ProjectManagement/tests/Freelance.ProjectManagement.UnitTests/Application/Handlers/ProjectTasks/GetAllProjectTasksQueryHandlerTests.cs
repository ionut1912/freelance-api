using System.Linq.Expressions;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Querires;
using Freelance.ProjectManagement.Domain.Entities;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.ProjectTasks;

public sealed class GetAllProjectTasksQueryHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsAllTasksAsDtos()
    {
        TaskRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<ProjectTask, object>>[]>()))
            .ReturnsAsync([TestData.Projects.Task()]);

        (await new GetAllProjectTasksQueryHandler(TaskRepository.Object).Handle(new GetAllProjectTasksQuery(), Ct))
            .Should().ContainSingle();
    }
}
