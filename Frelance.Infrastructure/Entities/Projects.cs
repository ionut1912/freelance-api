namespace Frelance.Infrastructure.Entities;

public class Projects
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
    public List<string> Technologies { get; set; } = [];
    public List<ProjectTasks> Tasks { get; set; } = [];
    public int UserId { get; set; }
    public Users User { get; set; }
    public float Budget { get; set; }
}