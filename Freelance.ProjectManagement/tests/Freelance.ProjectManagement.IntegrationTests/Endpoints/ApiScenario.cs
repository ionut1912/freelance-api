using System.Net.Http.Json;
using Freelance.ProjectManagement.Application.Dtos;

namespace Freelance.ProjectManagement.IntegrationTests.Endpoints;

/// <summary>Creates the resources other endpoint tests build on.</summary>
internal static class ApiScenario
{
    public static readonly DateTime Start = new(2030, 1, 1, 8, 0, 0, DateTimeKind.Utc);

    public static async Task<ProjectDto> CreateProjectAsync(HttpClient client, CancellationToken ct)
    {
        var response = await client.PostAsJsonAsync("/project", new
        {
            title = "Site",
            description = "Build site",
            deadline = Start.AddYears(1),
            amount = 1000,
            currency = "EUR",
            technologies = new[] { "dotnet", "react" }
        }, ct);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ProjectDto>(ct))!;
    }

    public static async Task<ProjectTaskDto> CreateTaskAsync(HttpClient client, Guid projectId, CancellationToken ct)
    {
        var response = await client.PostAsJsonAsync("/projectTask",
            new { projectId, title = "API", description = "Build API", status = "New", priority = "High" }, ct);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ProjectTaskDto>(ct))!;
    }

    public static async Task<TimeLogDto> CreateTimeLogAsync(HttpClient client, Guid taskId, CancellationToken ct)
    {
        var response = await client.PostAsJsonAsync("/timeLog",
            new { taskId, startTime = Start, endTime = Start.AddHours(3) }, ct);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<TimeLogDto>(ct))!;
    }
}
