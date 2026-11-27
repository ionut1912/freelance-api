using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Freelance.ProjectManagement.Application.Dtos;

namespace Freelance.ProjectManagement.IntegrationTests.Endpoints;

/// <summary>A client may only change the projects they created (and the tasks inside them).</summary>
public sealed class ProjectOwnershipTests(ProjectManagementApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;
    private readonly HttpClient _owner = factory.CreateClient("Client");
    private readonly HttpClient _intruder = factory.CreateClient("Client");

    private static object ProjectUpdate => new
    {
        title = "Hijacked",
        description = "Hijacked",
        deadline = ApiScenario.Start.AddYears(3),
        amount = 1,
        currency = "USD",
        technologies = new[] { "php" }
    };

    private static object TaskBody(Guid projectId) =>
        new { projectId, title = "Hijacked", description = "Hijacked", status = "Done", priority = "Low" };

    [Fact]
    public async Task CreateProject_RecordsTheCallerAsClient()
    {
        var accountId = Guid.NewGuid();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", ProjectManagementApiFactory.CreateToken("Client", accountId));

        var project = await ApiScenario.CreateProjectAsync(client, _ct);

        project.ClientId.Should().Be(accountId);
    }

    [Fact]
    public async Task UpdateProject_OfAnotherClient_ReturnsNotFoundAndKeepsIt()
    {
        var project = await ApiScenario.CreateProjectAsync(_owner, _ct);

        var response = await _intruder.PutAsJsonAsync($"/project/{project.Id}", ProjectUpdate, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await _owner.GetFromJsonAsync<ProjectDto>($"/project/{project.Id}", _ct))!.Title.Should().Be("Site");
    }

    [Fact]
    public async Task DeleteProject_OfAnotherClient_ReturnsNotFoundAndKeepsIt()
    {
        var project = await ApiScenario.CreateProjectAsync(_owner, _ct);

        var response = await _intruder.DeleteAsync($"/project/{project.Id}", _ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await _owner.GetAsync($"/project/{project.Id}", _ct)).StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateTask_InAnotherClientsProject_ReturnsNotFound()
    {
        var project = await ApiScenario.CreateProjectAsync(_owner, _ct);

        var response = await _intruder.PostAsJsonAsync("/projectTask", TaskBody(project.Id), _ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateTask_InAnotherClientsProject_ReturnsNotFoundAndKeepsIt()
    {
        var project = await ApiScenario.CreateProjectAsync(_owner, _ct);
        var task = await ApiScenario.CreateTaskAsync(_owner, project.Id, _ct);

        var response = await _intruder.PutAsJsonAsync($"/projectTask/{task.Id}", TaskBody(project.Id), _ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await _owner.GetFromJsonAsync<ProjectTaskDto>($"/projectTask/{task.Id}", _ct))!.Title.Should().Be("API");
    }

    [Fact]
    public async Task DeleteTask_InAnotherClientsProject_ReturnsNotFoundAndKeepsIt()
    {
        var project = await ApiScenario.CreateProjectAsync(_owner, _ct);
        var task = await ApiScenario.CreateTaskAsync(_owner, project.Id, _ct);

        var response = await _intruder.DeleteAsync($"/projectTask/{task.Id}", _ct);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await _owner.GetAsync($"/projectTask/{task.Id}", _ct)).StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateTask_AsFreelancer_IsStillAllowed()
    {
        var project = await ApiScenario.CreateProjectAsync(_owner, _ct);

        var task = await ApiScenario.CreateTaskAsync(factory.CreateClient("Freelancer"), project.Id, _ct);

        task.ProjectId.Should().Be(project.Id);
    }
}
