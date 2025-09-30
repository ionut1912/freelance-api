namespace Frelance.Infrastructure.Entities;

public class ProjectTechnologies:BaseEntity
{
    public int Id { get; init; }
    public string Technology { get; init; }
    public int ProjectId { get; init; }
    public Projects? Projects { get; init; }
}