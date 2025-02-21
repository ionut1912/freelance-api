namespace Frelance.Contracts.Dtos;

public record TimeLogDto(
    int Id,
    DateTime StartTime,
    DateTime EndTime,
    int TotalHours,
    DateTime CreatedAt,
    DateTime? UpdatedAt);