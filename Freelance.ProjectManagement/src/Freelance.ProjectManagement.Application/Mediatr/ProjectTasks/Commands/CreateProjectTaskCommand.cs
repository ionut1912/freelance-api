namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;

public sealed record CreateProjectTaskCommand(Guid ProjectId, string Title, string Description, string Status, string Priority, Guid AccountId);
