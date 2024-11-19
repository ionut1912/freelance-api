namespace Frelance.Infrastructure.Entities;

public class Project
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
    public List<string> Technologies { get; set; } = new List<string>();
    public float Budget { get; set; }
}