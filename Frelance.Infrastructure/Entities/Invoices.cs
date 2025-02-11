namespace Frelance.Infrastructure.Entities;

public class Invoices
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Projects Project { get; set; }
    public int ClientId { get; set; }
    public ClientProfiles Client { get; set; }
    public int FreelancerId { get; set; }
    public FreelancerProfiles Freelancer { get; set; }
    public DateOnly Date { get; set; }
    public decimal Amount { get; set; }
    public string InvoiceFileUrl { get; set; }
    public string Status { get; set; }
}