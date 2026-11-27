using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Handlers;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Querires;
using Freelance.ProjectManagement.Domain.Exceptions;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.ProjectTasks;

public sealed class GetProjectTaskByIdQueryHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_Missing_ThrowsProjectTaskNotFound()
    {
        GivenTask(null);

        await new GetProjectTaskByIdQueryHandler(TaskRepository.Object)
            .Invoking(h => h.Handle(new GetProjectTaskByIdQuery(Guid.NewGuid()), Ct)).Should().ThrowAsync<ProjectTaskNotFoundException>();
    }

    [Fact]
    public async Task Handle_Existing_ReturnsDto()
    {
        var task = TestData.Projects.Task();
        GivenTask(task);

        (await new GetProjectTaskByIdQueryHandler(TaskRepository.Object).Handle(new GetProjectTaskByIdQuery(task.Id), Ct))
            .Id.Should().Be(task.Id);
    }
}
