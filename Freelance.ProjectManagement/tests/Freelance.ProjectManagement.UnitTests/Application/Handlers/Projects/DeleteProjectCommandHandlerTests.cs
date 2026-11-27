using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;
using Freelance.ProjectManagement.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.Projects;

public sealed class DeleteProjectCommandHandlerTests : HandlerTestBase
{
    private DeleteProjectCommandHandler Handler =>
        new(ProjectRepository.Object, UnitOfWork.Object, NullLogger<DeleteProjectCommandHandler>.Instance);

    [Fact]
    public async Task Handle_Missing_ThrowsProjectNotFound()
    {
        GivenProject(null);

        await Handler.Invoking(h => h.Handle(new DeleteProjectCommand(Guid.NewGuid()), Ct)).Should().ThrowAsync<ProjectNotFoundException>();
        VerifyNotSaved();
    }

    [Fact]
    public async Task Handle_Existing_DeletesAndSaves()
    {
        var project = TestData.Projects.Project();
        GivenProject(project);

        await Handler.Handle(new DeleteProjectCommand(project.Id), Ct);

        ProjectRepository.Verify(r => r.Delete(project), Times.Once);
        VerifySaved();
    }
}
