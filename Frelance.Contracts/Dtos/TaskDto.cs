using Frelance.API.Frelance.Contracts.Enums;
using Frelance.API.Frelance.Domain.Entities;

namespace Frelance.API.Frelamce.Contracts;

public record TaskDto(int Id,string ProjectTitle,string Title,string Description, Status Status, Priority Priority,Project Project);