namespace Freelance.ProjectManagement.Application.Requests;

public record CreateProjectRequest(string Title, string Description, DateTime Deadline, decimal Amount, string Currency, List<string> Technologies)
{
}
