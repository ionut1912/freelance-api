using System.Net;
using System.Net.Http.Json;
using Freelance.ProjectManagement.Application.Dtos;

namespace Freelance.ProjectManagement.IntegrationTests.Endpoints;

public sealed class ProjectEndpointGroupTests(ProjectManagementApiFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient("Client");
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task Create_AsClient_ReturnsCreatedProject()
    {
        var project = await ApiScenario.CreateProjectAsync(_client, _ct);

        project.Should().BeEquivalentTo(new { Title = "Site", Amount = 1000m, Currency = "EUR" });
        project.Technologies.Select(t => t.Technology).Should().BeEquivalentTo("dotnet", "react");
    }

    [Fact]
    public async Task Create_AsFreelancer_IsForbidden() =>
        (await factory.CreateClient("Freelancer").PostAsJsonAsync("/project", new { title = "x" }, _ct))
        .StatusCode.Should().Be(HttpStatusCode.Forbidden);

    [Fact]
    public async Task Create_InvalidPayload_ReturnsValidationProblem()
    {
        var response = await _client.PostAsJsonAsync("/project",
            new { title = "", description = "", deadline = ApiScenario.Start, amount = 0, currency = "E", technologies = Array.Empty<string>() }, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await response.Content.ReadAsStringAsync(_ct)).Should().Contain("Validation Error");
    }

    [Fact]
    public async Task GetAll_And_GetById_ReturnTheProject()
    {
        var project = await ApiScenario.CreateProjectAsync(_client, _ct);
        var reader = factory.CreateClient("Freelancer");

        (await reader.GetFromJsonAsync<List<ProjectDto>>("/project", _ct)).Should().Contain(p => p.Id == project.Id);
        (await reader.GetFromJsonAsync<ProjectDto>($"/project/{project.Id}", _ct))!.Title.Should().Be("Site");
    }

    [Fact]
    public async Task GetById_Unknown_ReturnsNotFound() =>
        (await _client.GetAsync($"/project/{Guid.NewGuid()}", _ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);

    [Fact]
    public async Task Update_ChangesTheProject()
    {
        var project = await ApiScenario.CreateProjectAsync(_client, _ct);

        var response = await _client.PutAsJsonAsync($"/project/{project.Id}", new
        {
            title = "Site 2",
            description = "Build site 2",
            deadline = ApiScenario.Start.AddYears(2),
            amount = 1500,
            currency = "USD",
            technologies = new[] { "vue" }
        }, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var updated = await _client.GetFromJsonAsync<ProjectDto>($"/project/{project.Id}", _ct);
        updated.Should().BeEquivalentTo(new { Title = "Site 2", Amount = 1500m, Currency = "USD" });
        updated!.Technologies.Select(t => t.Technology).Should().Contain("vue");
    }

    [Fact]
    public async Task Delete_RemovesTheProject()
    {
        var project = await ApiScenario.CreateProjectAsync(_client, _ct);

        (await _client.DeleteAsync($"/project/{project.Id}", _ct)).StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await _client.GetAsync($"/project/{project.Id}", _ct)).StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Endpoints_WithoutToken_ReturnUnauthorized() =>
        (await factory.CreateClient().GetAsync("/project", _ct)).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
}
