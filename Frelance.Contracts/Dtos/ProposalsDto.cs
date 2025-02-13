namespace Frelance.Contracts.Dtos;

public record ProposalsDto(int Id, ProjectDto Project, UserProfileDto Proposer, decimal ProposedBudget, string Status, DateTime CreatedAt);