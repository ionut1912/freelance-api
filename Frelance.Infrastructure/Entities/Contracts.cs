namespace Frelance.Infrastructure.Entities;

public class Contracts
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public required Projects Project { get; set; }
    public int ClientId { get; set; }
    public required ClientProfiles Client { get; set; }
    public int FreelancerId { get; set; }
    public required FreelancerProfiles Freelancer { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Amount { get; set; }
    public required string ContractFileUrl { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}