using JetBrains.Annotations;

namespace Freelance.Contracts.Dtos;

[UsedImplicitly]
public record ContractsDto(
    int Id,
    ProjectDto Project,
    string ClientName,
    string FreelancerName,
    DateOnly StartDate,
    DateOnly EndDate,
    decimal Amount,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string ContractFile);