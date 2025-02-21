namespace Frelance.Contracts.Requests.ProjectTasks;

public record UpdateProjectTaskRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
}