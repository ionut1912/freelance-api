using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.Proposals;

[UsedImplicitly]
public class UpdateProposalRequest(decimal proposedBudget, string status)
{
    public decimal ProposedBudget { get; } = proposedBudget;
    public string? Status { get; } = status;
}