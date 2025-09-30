using JetBrains.Annotations;

namespace Freelance.Contracts.Dtos;

[UsedImplicitly]
public record TimeLogDto(
    int Id,
    DateTime StartTime,
    DateTime EndTime,
    int TotalHours,
    DateTime CreatedAt,
    DateTime? UpdatedAt);