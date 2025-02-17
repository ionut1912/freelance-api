namespace Frelance.Infrastructure.Entities;

public class FreelancerProfiles : BaseUserProfile
{
    public List<ProjectTasks> Tasks { get; set; } = [];
    public List<Skills> Skills { get; set; } = [];
    public List<FreelancerForeignLanguage> ForeignLanguages { get; set; } = [];
    public bool IsAvailable { get; set; }
    public required string Experience { get; set; }
    public int Rate { get; set; }
    public required string Currency { get; set; }
    public int Rating { get; set; }
    public required string PortfolioUrl { get; set; }

}