using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.Contracts;

[UsedImplicitly]
public record CreateContractRequest(
    string ProjectName,
    string FreelancerName,
    DateOnly StartDate,
    DateOnly EndDate,
    decimal Amount,
    string ContractFile);