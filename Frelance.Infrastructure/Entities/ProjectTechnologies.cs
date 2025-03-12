namespace Frelance.Infrastructure.Entities;

public class ProjectTechnologies
{
    public int Id { get; set; }
    public string? Technology { get; init; }
    public int ProjectId { get; set; }
    public Projects? Projects { get; init; }
}