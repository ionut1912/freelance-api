using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;
using Freelance.ProjectManagement.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.Projects;

public sealed class UpdateProjectCommandHandlerTests : HandlerTestBase
{
    private UpdateProjectCommandHandler Handler =>
        new(ProjectRepository.Object, UnitOfWork.Object, NullLogger<UpdateProjectCommandHandler>.Instance);

    private static UpdateProjectCommand Command(Guid id) => new(id, "New", "New desc", TestData.Projects.Deadline, 50, "USD", ["go"]);

    [Fact]
    public async Task Handle_Missing_ThrowsProjectNotFound()
    {
        GivenProject(null, tracked: true);

        await Handler.Invoking(h => h.Handle(Command(Guid.NewGuid()), Ct)).Should().ThrowAsync<ProjectNotFoundException>();
        VerifyNotSaved();
    }

    [Fact]
    public async Task Handle_Existing_UpdatesTrackedAggregateAndSaves()
    {
        var project = TestData.Projects.Project();
        GivenProject(project, tracked: true);

        await Handler.Handle(Command(project.Id), Ct);

        project.Title.Should().Be("New");
        project.Budget.Currency.Should().Be("USD");
        project.Technologies.Should().ContainSingle(t => t.Technology == "go");
        VerifySaved();
    }
}
