namespace Frelance.Contracts.Dtos;

public record InvoicesDto(int Id, ProjectDto Project, ClientProfileDto Client, FreelancerProfileDto Freelancer, DateOnly Date, decimal Amount, string InvoiceFileUrl, string Status);