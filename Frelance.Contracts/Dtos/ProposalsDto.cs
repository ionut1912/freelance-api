namespace Frelance.Contracts.Dtos;

public record ProposalsDto(int Id,ProjectDto Project, string Username, decimal ProposedBudget, string Status, DateTime CreatedAt, DateTime? UpdatedAt);