namespace Frelance.Infrastructure.Entities;

public class Proposals
{
    public int Id { get; init; }
    public int ProjectId { get; set; }
    public Projects? Project { get; init; }
    public int ProposerId { get; set; }
    public Users? Proposer { get; init; }
    public decimal ProposedBudget { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}