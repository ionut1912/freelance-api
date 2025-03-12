using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.Contracts;

[UsedImplicitly]
public class UpdateContractRequest(DateOnly endDate, decimal amount, string status, string contractFile)
{
    public DateOnly? EndDate { get; } = endDate;
    public decimal? Amount { get; } = amount;
    public string? Status { get; } = status;
    public string? ContractFile { get; } = contractFile;
}