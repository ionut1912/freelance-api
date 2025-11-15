namespace Freelancer.UserProfiles.Application.Requests;

public class CreateFreelancerProfileRequest : BaseCreateProfileRequest
{
    public string Experience { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string PortfolioUrl { get; set; }
    public List<string> ForeignLanguages { get; set; }
    public List<string> ProgrammingLanguages { get; set; }
    public List<string> Areas { get; set; }
}