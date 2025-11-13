namespace Freelancer.UserProfiles.Application.Requests;

public class UpdateFreelancerDetailRequest
{
    public List<string> ForeignLanguages { get; set; }
    public List<string> ProgrammingLanguages { get; set; }
    public List<string> Areas{get; set;}
    public string Experience { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string PortfolioUrl { get; set; }
}