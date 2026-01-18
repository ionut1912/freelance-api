namespace Freelance.ProjectManagement.Application.Requests;

public record CreateProjectTaskRequest(Guid ProjectId, string Title, string Description, string Status, string Priority)
{
}
