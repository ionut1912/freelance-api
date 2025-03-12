namespace Frelance.Infrastructure.Entities;

public class Projects
{
    public int Id { get; init; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime Deadline { get; set; }
    public List<ProjectTechnologies> Technologies { get; set; } = [];
    public List<ProjectTasks> Tasks { get; init; } = [];
    public List<Proposals> Proposals { get; init; } = [];
    public List<Contracts> Contracts { get; init; } = [];
    public List<Invoices> Invoices { get; init; } = [];
    public decimal Budget { get; set; }
}