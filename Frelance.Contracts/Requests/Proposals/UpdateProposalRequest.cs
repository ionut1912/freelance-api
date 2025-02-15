namespace Frelance.Contracts.Requests.Proposals;

public class UpdateProposalRequest
{
    public decimal? ProposedBudget { get; set; }
    public string? Status { get; set; }
}