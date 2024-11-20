using Microsoft.AspNetCore.Identity;

namespace Frelance.Infrastructure.Entities;

public class Users:IdentityUser<int>
{
    public Projects Projects { get; set; }
    public List<ProjectTasks> Tasks { get; set; }=new List<ProjectTasks>();
    public List<TimeLogs> TimeLogs { get; set; } = new List<TimeLogs>();
}