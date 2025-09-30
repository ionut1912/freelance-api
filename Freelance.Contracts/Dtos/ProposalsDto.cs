using JetBrains.Annotations;

namespace Freelance.Contracts.Dtos;

[UsedImplicitly]
public record ProposalsDto(
    int Id,
    ProjectDto Project,
    string Username,
    decimal ProposedBudget,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt);