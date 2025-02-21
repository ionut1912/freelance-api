namespace Frelance.Contracts.Dtos;

public record InvoicesDto(
    int Id,
    ProjectDto Project,
    string ClientName,
    string FreelancerName,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    decimal Amount,
    string Status,
    string InvoiceFile);