namespace Frelance.Contracts.Dtos;
public class FreelancerProfileDto : BaseProfileDto
{
    public List<TaskDto> Tasks { get; set; } = new();
    public List<SkillDto> Skills { get; set; } = new();
    public List<ForeignLanguageDto> ForeignLanguages { get; set; } = new();
    public bool IsAvailable { get; set; }
    public required string Experience { get; set; }
    public int Rate { get; set; }
    public required string Currency { get; set; }
    public int Rating { get; set; }
    public required string PortfolioUrl { get; set; }
}