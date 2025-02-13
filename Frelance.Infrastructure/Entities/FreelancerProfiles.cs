namespace Frelance.Infrastructure.Entities;

public class FreelancerProfiles : BaseUserProfile
{
    public List<ProjectTasks> Tasks { get; set; } = [];
    public List<Skiills> Skills { get; set; } = [];
    public List<string> ForeignLanguages { get; set; } = [];
    public bool IsAvailable { get; set; }
    public string Experience { get; set; }
    public int Rate { get; set; }
    public string Currency { get; set; }
    public int Rating { get; set; }
    public string PortfolioUrl { get; set; }

}