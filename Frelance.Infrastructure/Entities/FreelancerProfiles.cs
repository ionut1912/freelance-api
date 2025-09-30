namespace Frelance.Infrastructure.Entities;

public class FreelancerProfiles : BaseUserProfile
{
    public FreelancerProfiles() : base(null)
    {
    }

    public FreelancerProfiles(Users user) : base(user)
    {
    }

    public List<ProjectTasks> Tasks { get; init; } = [];
    public List<FreelancerProfileSkill> FreelancerProfileSkills { get; init; } = [];
    public List<FreelancerForeignLanguage> ForeignLanguages { get; set; } = [];
    public required string Experience { get; set; }
    public int Rate { get; set; }
    public required string Currency { get; set; }
    public int? Rating { get; set; }
    public required string PortfolioUrl { get; set; }
}