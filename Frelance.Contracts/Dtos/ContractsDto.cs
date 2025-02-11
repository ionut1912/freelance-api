namespace Frelance.Contracts.Dtos;

public record ContractsDto(int Id, ProjectDto Project, int ClientId, int FreelancerId, DateOnly StartDate, DateOnly EndDate, decimal Amount, string ContractFileUrl,string Status);