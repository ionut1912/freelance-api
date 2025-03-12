using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.Proposals;

[UsedImplicitly]
public class UpdateProposalRequest(decimal proposedBudget, string status)
{
    public decimal ProposedBudget { get; } = proposedBudget;
    public string? Status { get; } = status;
}