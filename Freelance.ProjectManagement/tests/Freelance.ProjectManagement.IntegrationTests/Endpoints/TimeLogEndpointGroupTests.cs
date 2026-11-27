using System.Net;
using System.Net.Http.Json;
using Freelance.ProjectManagement.Application.Dtos;

namespace Freelance.ProjectManagement.IntegrationTests.Endpoints;

public sealed class TimeLogEndpointGroupTests(ProjectManagementApiFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient("Client");
    private readonly HttpClient _freelancer = factory.CreateClient("Freelancer");
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    private async Task<ProjectTaskDto> CreateTaskAsync()
    {
        var project = await ApiScenario.CreateProjectAsync(_client, _ct);
        return await ApiScenario.CreateTaskAsync(_client, project.Id, _ct);
    }

    [Fact]
    public async Task Create_AsFreelancer_ComputesHours_AndAppearsOnTask()
    {
        var task = await CreateTaskAsync();

        var log = await ApiScenario.CreateTimeLogAsync(_freelancer, task.Id, _ct);

        log.TotalHours.Should().Be(3);
        (await _client.GetFromJsonAsync<ProjectTaskDto>($"/projectTask/{task.Id}", _ct))!.TimeLogs
            .Should().ContainSingle(l => l.Id == log.Id);
    }

    [Fact]
    public async Task Create_AsClient_IsForbidden() =>
        (await _client.PostAsJsonAsync("/timeLog", new { taskId = Guid.NewGuid() }, _ct)).StatusCode.Should().Be(HttpStatusCode.Forbidden);

    [Fact]
    public async Task Create_EndBeforeStart_ReturnsBadRequest() =>
        (await _freelancer.PostAsJsonAsync("/timeLog",
            new { taskId = Guid.NewGuid(), startTime = ApiScenario.Start, endTime = ApiScenario.Start.AddHours(-1) }, _ct))
        .StatusCode.Should().Be(HttpStatusCode.BadRequest);

    [Fact]
    public async Task Update_ChangesHours()
    {
        var task = await CreateTaskAsync();
        var log = await ApiScenario.CreateTimeLogAsync(_freelancer, task.Id, _ct);

        var response = await _freelancer.PutAsJsonAsync($"/timeLog/{log.Id}",
            new { taskId = task.Id, startTime = ApiScenario.Start, endTime = ApiScenario.Start.AddHours(5) }, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await _freelancer.GetFromJsonAsync<TimeLogDto>($"/timeLog/{log.Id}", _ct))!.TotalHours.Should().Be(5);
    }

    [Fact]
    public async Task Update_LogOfAnotherTask_ReturnsNotFound()
    {
        var task = await CreateTaskAsync();

        (await _freelancer.PutAsJsonAsync($"/timeLog/{Guid.NewGuid()}",
            new { taskId = task.Id, startTime = ApiScenario.Start, endTime = ApiScenario.Start.AddHours(1) }, _ct))
            .StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_And_Delete()
    {
        var task = await CreateTaskAsync();
        var log = await ApiScenario.CreateTimeLogAsync(_freelancer, task.Id, _ct);

        (await _client.GetFromJsonAsync<List<TimeLogDto>>("/timeLog", _ct)).Should().Contain(l => l.Id == log.Id);
        (await _freelancer.DeleteAsync($"/timeLog/{log.Id}", _ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await _freelancer.GetAsync($"/timeLog/{log.Id}", _ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await _freelancer.DeleteAsync($"/timeLog/{log.Id}", _ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
