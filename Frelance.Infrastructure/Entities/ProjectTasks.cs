using Frelance.Contracts.Dtos;
using Frelance.Contracts.Enums;
namespace Frelance.Infrastructure.Entities;

public class ProjectTasks
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Projects Projects { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<TimeLogs> TimeLogs { get; set; } = new List<TimeLogs>();
    public int UserId { get; set; }
    public Users Users { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public Priority Priority { get; set; }
}