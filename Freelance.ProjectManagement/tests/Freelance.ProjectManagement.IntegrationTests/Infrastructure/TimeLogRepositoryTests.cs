using Freelance.ProjectManagement.Domain.Interfaces;
using Freelance.ProjectManagement.IntegrationTests.Endpoints;
using Microsoft.Extensions.DependencyInjection;

namespace Freelance.ProjectManagement.IntegrationTests.Infrastructure;

public sealed class TimeLogRepositoryTests(ProjectManagementApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task GetByIdWithTracking_ReturnsTheStoredLog()
    {
        var client = factory.CreateClient("Client");
        var project = await ApiScenario.CreateProjectAsync(client, _ct);
        var task = await ApiScenario.CreateTaskAsync(client, project.Id, _ct);
        var log = await ApiScenario.CreateTimeLogAsync(factory.CreateClient("Freelancer"), task.Id, _ct);
        using var scope = factory.Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<ITimeLogRepository>();

        (await repository.GetByIdWithTrackingAsync(log.Id, _ct))!.TotalHours.Should().Be(3);
        (await repository.GetByIdWithTrackingAsync(Guid.NewGuid(), _ct)).Should().BeNull();
    }
}
