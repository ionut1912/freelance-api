namespace Freelance.Infrastructure.Entities;

public class TimeLogs : BaseEntity
{
    public int Id { get; init; }
    public int TaskId { get; set; }
    public ProjectTasks? Task { get; init; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int FreelancerProfileId { get; set; }
    public int TotalHours { get; set; }
}