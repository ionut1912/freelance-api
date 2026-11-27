using Freelance.ProjectManagement.Domain.Interfaces;
using Freelance.ProjectManagement.IntegrationTests.Endpoints;
using Microsoft.Extensions.DependencyInjection;

namespace Freelance.ProjectManagement.IntegrationTests.Infrastructure;

public sealed class ProjectTaskRepositoryTests(ProjectManagementApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task GetByIdWithTracking_IncludesTimeLogs()
    {
        var client = factory.CreateClient("Client");
        var project = await ApiScenario.CreateProjectAsync(client, _ct);
        var task = await ApiScenario.CreateTaskAsync(client, project.Id, _ct);
        var log = await ApiScenario.CreateTimeLogAsync(factory.CreateClient("Freelancer"), task.Id, _ct);
        using var scope = factory.Services.CreateScope();

        var loaded = await scope.ServiceProvider.GetRequiredService<IProjectTaskRepository>()
            .GetByIdWithTrackingAsync(task.Id, _ct, t => t.TimeLogs);

        loaded!.TimeLogs.Should().ContainSingle(l => l.Id == log.Id);
    }
}
