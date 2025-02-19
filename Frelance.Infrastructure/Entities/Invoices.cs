namespace Frelance.Infrastructure.Entities;

public class Invoices
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Projects? Project { get; set; }
    public int ClientId { get; set; }
    public ClientProfiles? Client { get; set; }
    public int FreelancerId { get; set; }
    public FreelancerProfiles? Freelancer { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public decimal Amount { get; set; }
    public required string Status { get; set; }
    public required string InvoiceFile { get; set; }
}