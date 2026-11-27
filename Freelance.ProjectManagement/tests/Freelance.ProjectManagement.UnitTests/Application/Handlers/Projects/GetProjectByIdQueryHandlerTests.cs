using Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Queries;
using Freelance.ProjectManagement.Domain.Exceptions;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.Projects;

public sealed class GetProjectByIdQueryHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_Missing_ThrowsProjectNotFound()
    {
        GivenProject(null);

        await new GetProjectByIdQueryHandler(ProjectRepository.Object)
            .Invoking(h => h.Handle(new GetProjectByIdQuery(Guid.NewGuid()), Ct)).Should().ThrowAsync<ProjectNotFoundException>();
    }

    [Fact]
    public async Task Handle_Existing_ReturnsDto()
    {
        var project = TestData.Projects.Project();
        GivenProject(project);

        var dto = await new GetProjectByIdQueryHandler(ProjectRepository.Object).Handle(new GetProjectByIdQuery(project.Id), Ct);

        dto.Id.Should().Be(project.Id);
    }
}
