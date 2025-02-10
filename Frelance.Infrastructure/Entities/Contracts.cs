namespace Frelance.Infrastructure.Entities;

public class Contracts
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Projects Project { get; set; }
    public int ClientId { get; set; }
    public ClientProfiles Client { get; set; }
    public int FreelancerId { get; set; }
    public FreelancerProfiles Freelancer { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int Amount { get; set; }
    public string ContractFileUrl { get; set; }
}