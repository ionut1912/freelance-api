namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;

public sealed record CreateTimeLogCommand(Guid TaskId, DateTime StartTime, DateTime EndTime);
