using System.Net.Http.Json;
using Freelance.ProjectManagement.Application.Dtos;
using Freelance.ProjectManagement.IntegrationTests.Endpoints;

namespace Freelance.ProjectManagement.IntegrationTests.Infrastructure;

public sealed class TimeLogConfigurationsTests(ProjectManagementApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    /// <summary>Timestamps sent with an offset are stored as the same instant in UTC.</summary>
    [Fact]
    public async Task Configure_TimestampsWithOffset_RoundTripAsTheSameInstant()
    {
        var client = factory.CreateClient("Client");
        var freelancer = factory.CreateClient("Freelancer");
        var project = await ApiScenario.CreateProjectAsync(client, _ct);
        var task = await ApiScenario.CreateTaskAsync(client, project.Id, _ct);

        var response = await freelancer.PostAsJsonAsync("/timeLog",
            new { taskId = task.Id, startTime = "2030-03-31T01:00:00+02:00", endTime = "2030-03-31T05:00:00+03:00" }, _ct);
        response.EnsureSuccessStatusCode();
        var created = (await response.Content.ReadFromJsonAsync<TimeLogDto>(_ct))!;
        var stored = (await freelancer.GetFromJsonAsync<TimeLogDto>($"/timeLog/{created.Id}", _ct))!;

        stored.StartTime.Should().Be(new DateTime(2030, 3, 30, 23, 0, 0, DateTimeKind.Utc));
        stored.EndTime.Should().Be(new DateTime(2030, 3, 31, 2, 0, 0, DateTimeKind.Utc));
        stored.TotalHours.Should().Be(3);
    }
}
