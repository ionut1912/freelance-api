namespace Freelance.Infrastructure.Entities;

public class Invoices : BaseEntity
{
    public int Id { get; init; }
    public int ProjectId { get; set; }
    public Projects? Project { get; init; }
    public int ClientId { get; set; }
    public ClientProfiles? Client { get; init; }
    public int FreelancerId { get; set; }
    public FreelancerProfiles? Freelancer { get; init; }
    public decimal Amount { get; set; }
    public required string Status { get; set; }
    public required string InvoiceFile { get; set; }
}