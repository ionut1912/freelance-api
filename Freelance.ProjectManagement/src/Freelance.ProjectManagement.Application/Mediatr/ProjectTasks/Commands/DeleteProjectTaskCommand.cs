namespace Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;

public sealed record DeleteProjectTaskCommand(Guid Id, Guid ClientId);
