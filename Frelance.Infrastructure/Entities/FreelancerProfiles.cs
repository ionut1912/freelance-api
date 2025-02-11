namespace Frelance.Infrastructure.Entities;

public class FreelancerProfiles : BaseUserProfile
{
    public List<Projects> Projects { get; set; } = [];
    public List<ProjectTasks> Tasks { get; set; } = [];
    public List<TimeLogs> TimeLogs { get; set; } = [];
    public List<Skiills> Skills { get; set; } = [];
    public List<string> ForeignLanguages { get; set; } = [];
    public bool IsAvailable { get; set; }
    public string? Experience { get; set; }
    public int Rate { get; set; }
    public required string Currency { get; set; }
    public int Rating { get; set; }
    public required string PortofolioUrl { get; set; }
}