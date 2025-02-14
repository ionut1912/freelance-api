namespace Frelance.Contracts.Requests.Proposals;

public record CreateProposalRequest(string ProjectName, decimal ProposedBudget);