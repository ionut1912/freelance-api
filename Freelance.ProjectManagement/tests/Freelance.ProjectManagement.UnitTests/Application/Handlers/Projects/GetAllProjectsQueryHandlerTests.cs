using System.Linq.Expressions;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Queries;
using Freelance.ProjectManagement.Domain.Entities;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.Projects;

public sealed class GetAllProjectsQueryHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_ReturnsAllProjectsAsDtos()
    {
        ProjectRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>(), It.IsAny<Expression<Func<Project, object>>[]>()))
            .ReturnsAsync([TestData.Projects.Project(), TestData.Projects.Project()]);

        var dtos = await new GetAllProjectsQueryHandler(ProjectRepository.Object).Handle(new GetAllProjectsQuery(), Ct);

        dtos.Should().HaveCount(2);
    }
}
