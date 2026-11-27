using System.Net;
using System.Net.Http.Json;
using Freelance.ProjectManagement.Application.Dtos;

namespace Freelance.ProjectManagement.IntegrationTests.Endpoints;

/// <summary>Input the database cannot store must be rejected as 400, never surface as 500.</summary>
public sealed class ProjectInputValidationTests(ProjectManagementApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;
    private readonly HttpClient _client = factory.CreateClient("Client");

    private static string Long(int length) => new('a', length);

    private static Dictionary<string, object?> ProjectBody() => new()
    {
        ["title"] = "Site",
        ["description"] = "Build site",
        ["deadline"] = ApiScenario.Start.AddYears(1),
        ["amount"] = 1000,
        ["currency"] = "EUR",
        ["technologies"] = new[] { "dotnet" }
    };

    public static TheoryData<string, object> UnstorableProjectFields => new()
    {
        { "title", Long(51) },
        { "description", Long(51) },
        { "technologies", new[] { Long(101) } },
        { "technologies", new[] { " " } }
    };

    [Theory]
    [MemberData(nameof(UnstorableProjectFields))]
    public async Task CreateProject_WithUnstorableValue_ReturnsBadRequest(string field, object value)
    {
        var body = ProjectBody();
        body[field] = value;

        var response = await _client.PostAsJsonAsync("/project", body, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("title")]
    [InlineData("description")]
    public async Task UpdateProject_WithTextLongerThanColumn_ReturnsBadRequest(string field)
    {
        var project = await ApiScenario.CreateProjectAsync(_client, _ct);
        var body = ProjectBody();
        body[field] = Long(51);

        var response = await _client.PutAsJsonAsync($"/project/{project.Id}", body, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("deadline")]
    [InlineData("currency")]
    public async Task UpdateProject_WithoutRequiredField_ReturnsBadRequest(string field)
    {
        var project = await ApiScenario.CreateProjectAsync(_client, _ct);
        var body = ProjectBody();
        body.Remove(field);

        var response = await _client.PutAsJsonAsync($"/project/{project.Id}", body, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateProject_ReplacesTechnologies()
    {
        var project = await ApiScenario.CreateProjectAsync(_client, _ct);
        var body = ProjectBody();
        body["technologies"] = new[] { "react", "vue" };

        (await _client.PutAsJsonAsync($"/project/{project.Id}", body, _ct)).EnsureSuccessStatusCode();

        (await _client.GetFromJsonAsync<ProjectDto>($"/project/{project.Id}", _ct))!.Technologies
            .Select(t => t.Technology).Should().BeEquivalentTo("react", "vue");
    }

    [Fact]
    public async Task UpdateProject_WithoutTechnologies_KeepsThem()
    {
        var project = await ApiScenario.CreateProjectAsync(_client, _ct);
        var body = ProjectBody();
        body.Remove("technologies");

        var response = await _client.PutAsJsonAsync($"/project/{project.Id}", body, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await _client.GetFromJsonAsync<ProjectDto>($"/project/{project.Id}", _ct))!.Technologies.Should().HaveCount(2);
    }

    [Theory]
    [InlineData(101, 10)]
    [InlineData(10, 501)]
    public async Task CreateTask_WithTextLongerThanColumn_ReturnsBadRequest(int titleLength, int descriptionLength)
    {
        var project = await ApiScenario.CreateProjectAsync(_client, _ct);

        var response = await _client.PostAsJsonAsync("/projectTask", new
        {
            projectId = project.Id,
            title = Long(titleLength),
            description = Long(descriptionLength),
            status = "New",
            priority = "Low"
        }, _ct);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
