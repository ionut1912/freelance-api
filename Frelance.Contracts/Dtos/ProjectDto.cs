using JetBrains.Annotations;

namespace Frelance.Contracts.Dtos;

[UsedImplicitly]
public record ProjectDto(
    int Id,
    string Title,
    string Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime Deadline,
    List<ProjectTechnologiesDto> Technologies,
    decimal Budget,
    List<TaskDto> Tasks);