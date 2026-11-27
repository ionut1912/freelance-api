namespace Freelance.ProjectManagement.Application.Requests;

public sealed record CreateProjectRequest(string Title, string Description, DateTime Deadline, decimal Amount, string Currency, List<string> Technologies);
