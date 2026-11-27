using Freelance.ProjectManagement.Domain.Entities;
using Freelance.ProjectManagement.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Interfaces;

namespace Freelance.ProjectManagement.IntegrationTests.Infrastructure;

public sealed class ProjectRepositoryTests(ProjectManagementApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task AddAndDelete_ArePersistedByTheUnitOfWork()
    {
        var project = Project.Create("Site", "Build", new DateTime(2030, 1, 1, 0, 0, 0, DateTimeKind.Utc), 1, "EUR");
        using (var scope = factory.Services.CreateScope())
        {
            await scope.ServiceProvider.GetRequiredService<IProjectRepository>().AddAsync(project, _ct);
            await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().SaveChangesAsync(_ct);
        }

        using (var scope = factory.Services.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IProjectRepository>();
            var loaded = await repository.GetByIdAsync(project.Id, _ct);
            loaded!.Title.Should().Be("Site");
            repository.Delete(loaded);
            await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().SaveChangesAsync(_ct);
        }

        using (var scope = factory.Services.CreateScope())
            (await scope.ServiceProvider.GetRequiredService<IProjectRepository>().GetByIdAsync(project.Id, _ct))
                .Should().BeNull();
    }

    [Fact]
    public async Task GetByIdWithTracking_CancelledToken_Throws()
    {
        using var scope = factory.Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IProjectRepository>();

        await FluentActions.Awaiting(() => repository.GetByIdWithTrackingAsync(Guid.NewGuid(), new CancellationToken(true)))
            .Should().ThrowAsync<OperationCanceledException>();
    }
}
