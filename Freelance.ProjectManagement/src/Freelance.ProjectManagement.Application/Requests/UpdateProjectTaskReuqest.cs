namespace Freelance.ProjectManagement.Application.Requests;

public record UpdateProjectTaskReuqest(Guid ProjectId,string Title, string Description, string Status, string Priority)
{
}
