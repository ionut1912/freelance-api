using Frelance.Contracts.Enums;

namespace Frelance.Contracts.Dtos;

public record TaskDto(int Id, string Title, string Description, string ProjectTaskStatus, string Priority, List<TimeLogDto> TimeLogs, DateTime CreatedAt, DateTime? UpdatedAt);