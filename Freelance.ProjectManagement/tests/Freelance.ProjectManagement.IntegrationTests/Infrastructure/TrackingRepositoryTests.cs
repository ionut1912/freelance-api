using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Interfaces;
using Freelance.ProjectManagement.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.IntegrationTests.Infrastructure;

public sealed class TrackingRepositoryTests(ProjectManagementApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task GetByIdWithTracking_ChangesArePersistedWithoutCallingUpdate()
    {
        var project = Project.Create("Tracked", "desc", new DateTime(2031, 1, 1, 0, 0, 0, DateTimeKind.Utc), 10, "EUR");
        using (var scope = factory.Services.CreateScope())
        {
            await scope.ServiceProvider.GetRequiredService<IProjectRepository>().AddAsync(project, _ct);
            await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().SaveChangesAsync(_ct);
        }

        using (var scope = factory.Services.CreateScope())
        {
            var tracked = await scope.ServiceProvider.GetRequiredService<IProjectRepository>()
                .GetByIdWithTrackingAsync(project.Id, _ct, p => p.Technologies);
            tracked!.Update("Renamed", "", tracked.Deadline, 20, "EUR");
            tracked.AddTechnologies([ProjectTechnology.Create("rust")]);
            await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().SaveChangesAsync(_ct);
        }

        using var readScope = factory.Services.CreateScope();
        var reloaded = await readScope.ServiceProvider.GetRequiredService<IProjectRepository>()
            .GetByIdAsync(project.Id, _ct, p => p.Technologies);
        reloaded!.Title.Should().Be("Renamed");
        reloaded.Technologies.Should().ContainSingle(t => t.Technology == "rust");
    }

    [Fact]
    public async Task GetByIdWithTracking_Unknown_ReturnsNull()
    {
        using var scope = factory.Services.CreateScope();

        (await scope.ServiceProvider.GetRequiredService<IProjectRepository>().GetByIdWithTrackingAsync(Guid.NewGuid(), _ct))
            .Should().BeNull();
        (await scope.ServiceProvider.GetRequiredService<IProjectTaskRepository>().GetByIdWithTrackingAsync(Guid.NewGuid(), _ct, null!))
            .Should().BeNull();
    }
}
