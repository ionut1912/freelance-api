using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.Proposals;

[UsedImplicitly]
public record CreateProposalRequest(string ProjectName, decimal ProposedBudget);