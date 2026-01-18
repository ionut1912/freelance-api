namespace Freelance.ProjectManagement.Application.Requests;

public record CreateTimeLogRequest(Guid TaskId, DateTime StartTime, DateTime EndTime)
{
}
