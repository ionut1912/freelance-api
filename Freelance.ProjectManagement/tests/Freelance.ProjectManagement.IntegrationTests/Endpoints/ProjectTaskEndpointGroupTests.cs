using System.Net;
using System.Net.Http.Json;
using Freelance.ProjectManagement.Application.Dtos;

namespace Freelance.ProjectManagement.IntegrationTests.Endpoints;

public sealed class ProjectTaskEndpointGroupTests(ProjectManagementApiFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient("Client");
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task Create_ReturnsTaskLinkedToProject()
    {
        var project = await ApiScenario.CreateProjectAsync(_client, _ct);

        var task = await ApiScenario.CreateTaskAsync(_client, project.Id, _ct);

        task.Should().BeEquivalentTo(new { ProjectId = project.Id, Title = "API", Status = "New", Priority = "High" });
        (await _client.GetFromJsonAsync<ProjectDto>($"/project/{project.Id}", _ct))!.ProjectTasks
            .Should().ContainSingle(t => t.Id == task.Id);
    }

    [Fact]
    public async Task Create_InvalidStatus_ReturnsBadRequest() =>
        (await _client.PostAsJsonAsync("/projectTask",
            new { projectId = Guid.NewGuid(), title = "x", description = "y", status = "Bogus", priority = "High" }, _ct))
        .StatusCode.Should().Be(HttpStatusCode.BadRequest);

    [Fact]
    public async Task Create_ForUnknownProject_ReturnsNotFound() =>
        (await _client.PostAsJsonAsync("/projectTask",
            new { projectId = Guid.NewGuid(), title = "x", description = "y", status = "New", priority = "High" }, _ct))
        .StatusCode.Should().Be(HttpStatusCode.NotFound);

    [Fact]
    public async Task Update_ThenGet_ReturnsUpdatedTask()
    {
        var project = await ApiScenario.CreateProjectAsync(_client, _ct);
        var task = await ApiScenario.CreateTaskAsync(_client, project.Id, _ct);

        var response = await _client.PutAsJsonAsync($"/projectTask/{task.Id}",
            new { projectId = project.Id, title = "API v2", description = "", status = "InProgress", priority = "Medium" }, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await _client.GetFromJsonAsync<ProjectTaskDto>($"/projectTask/{task.Id}", _ct))
            .Should().BeEquivalentTo(new { Title = "API v2", Description = "Build API", Status = "InProgress", Priority = "Medium" });
    }

    [Fact]
    public async Task GetAll_ContainsCreatedTask()
    {
        var project = await ApiScenario.CreateProjectAsync(_client, _ct);
        var task = await ApiScenario.CreateTaskAsync(_client, project.Id, _ct);

        (await _client.GetFromJsonAsync<List<ProjectTaskDto>>("/projectTask", _ct)).Should().Contain(t => t.Id == task.Id);
    }

    [Fact]
    public async Task GetById_Unknown_ReturnsNotFound() =>
        (await _client.GetAsync($"/projectTask/{Guid.NewGuid()}", _ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);

    [Fact]
    public async Task Delete_AsFreelancer_IsForbidden_AsClient_Succeeds()
    {
        var project = await ApiScenario.CreateProjectAsync(_client, _ct);
        var task = await ApiScenario.CreateTaskAsync(_client, project.Id, _ct);

        (await factory.CreateClient("Freelancer").DeleteAsync($"/projectTask/{task.Id}", _ct)).StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await _client.DeleteAsync($"/projectTask/{task.Id}", _ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await _client.GetAsync($"/projectTask/{task.Id}", _ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
