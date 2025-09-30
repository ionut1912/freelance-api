using JetBrains.Annotations;

namespace Freelance.Contracts.Dtos;

[UsedImplicitly]
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