namespace Freelance.ProjectManagement.Application.Dtos;

public sealed record ProjectTaskDto(
    Guid Id,
    Guid ProjectId,
    string Title,
    string Description,
    List<TimeLogDto> TimeLogs,
    Guid FreelancerId,
    string Status,
    string Priority);
