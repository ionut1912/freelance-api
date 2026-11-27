namespace Freelance.ProjectManagement.Application.Dtos;

public sealed record TimeLogDto(Guid Id, Guid TaskId, DateTime StartTime, DateTime EndTime, int TotalHours);
