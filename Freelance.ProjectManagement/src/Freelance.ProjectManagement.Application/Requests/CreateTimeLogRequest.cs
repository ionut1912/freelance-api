namespace Freelance.ProjectManagement.Application.Requests;

public sealed record CreateTimeLogRequest(Guid TaskId, DateTime StartTime, DateTime EndTime);
