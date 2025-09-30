namespace Freelance.Infrastructure.Entities;

public class Contracts : BaseEntity
{
    public int Id { get; init; }
    public int ProjectId { get; set; }
    public required Projects Project { get; init; }
    public int ClientId { get; set; }
    public required ClientProfiles Client { get; init; }
    public int FreelancerId { get; set; }
    public required FreelancerProfiles Freelancer { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; set; }
    public decimal Amount { get; set; }
    public required string Status { get; set; }
    public required string ContractFile { get; set; }
}