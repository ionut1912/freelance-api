namespace Frelance.Infrastructure.Entities;

public class Invoices
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public required Projects Project { get; set; }
    public int ClientId { get; set; }
    public required ClientProfiles Client { get; set; }
    public int FreelancerId { get; set; }
    public required FreelancerProfiles Freelancer { get; set; }
    public DateOnly Date { get; set; }
    public decimal Amount { get; set; }
    public required string InvoiceFileUrl { get; set; }
}