namespace Freelance.ProjectManagement.Application.Requests;

public record UpdateTimeLogRequest(Guid TaskId,DateTime StartTime, DateTime EndTime)
{
}
