namespace Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;

public sealed record CreateProjectCommand(string Title, string Description, DateTime Deadline, decimal Amount, string Currency, List<string> Technologies, Guid ClientId);
