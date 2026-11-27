namespace Freelance.ProjectManagement.Application.Requests;

public sealed record UpdateTimeLogRequest(Guid TaskId, DateTime StartTime, DateTime EndTime);
