using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.Proposals;

[UsedImplicitly]
public record CreateProposalRequest(string ProjectName, decimal ProposedBudget);