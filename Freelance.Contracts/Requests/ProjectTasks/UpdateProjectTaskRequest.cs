using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.ProjectTasks;

[UsedImplicitly]
public record UpdateProjectTaskRequest
{
    public UpdateProjectTaskRequest(string title, string description, string status, string priority)
    {
        Title = title;
        Description = description;
        Status = status;
        Priority = priority;
    }

    public string? Title { get; }
    public string? Description { get; }
    public string? Status { get; }
    public string? Priority { get; }
}