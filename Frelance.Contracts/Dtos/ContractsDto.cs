namespace Frelance.Contracts.Dtos;

public record ContractsDto(int Id, ProjectDto Project, string ClientName, string FreelancerName, DateOnly StartDate, DateOnly EndDate, decimal Amount, string ContractFileUrl, string Status, DateTime CreatedAt, DateTime? UpdatedAt);