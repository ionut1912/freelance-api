using Frelance.Contracts.Dtos;
using Frelance.Contracts.Enums;
namespace Frelance.Infrastructure.Entities;

public class ProjectTask
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
    public ProjectTaskStatus Status { get; set; }
    public Priority Priority { get; set; }
}