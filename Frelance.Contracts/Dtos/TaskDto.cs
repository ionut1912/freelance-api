using Frelance.Contracts.Enums;

namespace Frelance.Contracts.Dtos;

public record TaskDto(int Id,string Title,string Description, ProjectTaskStatus ProjectTaskStatus, Priority Priority);