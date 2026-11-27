namespace Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;

public sealed record UpdateTimeLogCommand(Guid Id, Guid TaskId, DateTime StartTime, DateTime EndTime, Guid FreelancerId);
