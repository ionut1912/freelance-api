namespace Frelance.Contracts.Dtos;

public record InvoicesDto(int Id, ProjectDto Project, string ClientName, string FreelancerName, DateOnly Date, decimal Amount, string InvoiceFileUrl, string Status);