using Freelance.ProjectManagement.Domain.ValueObjects;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;
using Freelance.ProjectManagement.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.Projects;

public sealed class UpdateProjectCommandHandlerTests : HandlerTestBase
{
    private UpdateProjectCommandHandler Handler =>
        new(ProjectRepository.Object, UnitOfWork.Object, NullLogger<UpdateProjectCommandHandler>.Instance);

    private static UpdateProjectCommand Command(Guid id, Guid clientId = default, List<string>? technologies = null) =>
        new(id, "New", "New desc", TestData.Projects.Deadline, 50, "USD", technologies ?? ["go"], clientId);

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

    [Fact]
    public async Task Handle_ProjectOfAnotherClient_ThrowsProjectNotFoundWithoutSaving()
    {
        var project = TestData.Projects.Project();
        GivenProject(project, tracked: true);

        await Handler.Invoking(h => h.Handle(Command(project.Id, Guid.NewGuid()), Ct))
            .Should().ThrowAsync<ProjectNotFoundException>();
        VerifyNotSaved();
    }

    [Fact]
    public async Task Handle_WithoutTechnologies_KeepsExistingOnes()
    {
        var project = TestData.Projects.Project();
        project.AddTechnologies([ProjectTechnology.Create("dotnet")]);
        GivenProject(project, tracked: true);

        await Handler.Handle(Command(project.Id, technologies: []), Ct);

        project.Technologies.Should().ContainSingle(t => t.Technology == "dotnet");
        VerifySaved();
    }
}
