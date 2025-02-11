namespace Frelance.Infrastructure.Entities;

public class Proposals
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public required Projects Project { get; set; }
    public int ProposerId { get; set; }
    public required Users Proposer { get; set; }
    public decimal ProposedBudget { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}