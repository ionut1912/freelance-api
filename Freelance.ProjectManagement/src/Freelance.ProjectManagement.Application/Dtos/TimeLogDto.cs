namespace Freelance.ProjectManagement.Application.Dtos;

public record TimeLogDto(Guid Id, Guid TaskId, DateTime StartTime, DateTime EndTime, int TotalHours)
{
}
