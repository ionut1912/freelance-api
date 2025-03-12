namespace Frelance.Infrastructure.Entities;

public class ProjectTasks
{
    public int Id { get; init; }
    public int ProjectId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public List<TimeLogs> TimeLogs { get; init; } = [];
    public int FreelancerProfileId { get; set; }
    public FreelancerProfiles? FreelancerProfiles { get; init; }
    public required string Status { get; set; }
    public required string Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}