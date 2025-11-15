namespace Freelancer.UserProfiles.Application.Dtos;

public class FreelancerProfileDto : BaseProfileDto
{
    public string Experience { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public int? Rating { get; set; }
    public string PortfolioUrl { get; set; }
    public List<string> ForeignLanguages { get; set; }
    public List<string> ProgrammingLanguages { get; set; }
    public List<string> Areas { get; set; }
}