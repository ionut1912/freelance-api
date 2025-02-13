namespace Frelance.Infrastructure.Entities;

public class Projects
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
    public List<string> Technologies { get; set; } = new();
    public List<ProjectTasks> Tasks { get; set; } = new();
    public List<Proposals> Proposals { get; set; } = new();
    public List<Contracts> Contracts { get; set; } = new();
    public List<Invoices> Invoices { get; set; } = new();
    public decimal Budget { get; set; }
}