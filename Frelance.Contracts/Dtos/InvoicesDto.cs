namespace Frelance.Contracts.Dtos;

public record InvoicesDto(int Id, ProjectDto Project, int ClientId, int FreelancerId, DateOnly Date, decimal Amount, string InvoiceFileUrl, string Status);