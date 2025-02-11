namespace Frelance.Contracts.Dtos;

public record ProposalsDto(int Id, ProjectDto Project, int ProposerId, decimal ProposedBudget, string Status, DateTime CreatedAt);