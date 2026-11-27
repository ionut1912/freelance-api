using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Handlers;
using Freelance.ProjectManagement.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.ProjectManagement.UnitTests.Application.Handlers.Projects;

public sealed class CreateProjectCommandHandlerTests : HandlerTestBase
{
    [Fact]
    public async Task Handle_PersistsProjectWithTechnologies_AndReturnsDto()
    {
        var handler = new CreateProjectCommandHandler(ProjectRepository.Object, UnitOfWork.Object, NullLogger<CreateProjectCommandHandler>.Instance);

        var dto = await handler.Handle(new CreateProjectCommand("Site", "Build", TestData.Projects.Deadline, 100, "EUR", ["dotnet", "dotnet", "vue"]), Ct);

        ProjectRepository.Verify(r => r.AddAsync(It.Is<Project>(p => p.Technologies.Count == 2), Ct), Times.Once);
        VerifySaved();
        dto.Title.Should().Be("Site");
        dto.Technologies.Select(t => t.Technology).Should().Equal("dotnet", "vue");
    }
}
