namespace Frelance.Contracts.Dtos;

public record ContractsDto(int Id, ProjectDto Project, ClientProfileDto Client, FreelancerProfileDto Freelancer, DateOnly StartDate, DateOnly EndDate, decimal Amount, string ContractFileUrl, string Status);