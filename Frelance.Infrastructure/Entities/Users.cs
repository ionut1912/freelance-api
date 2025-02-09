using Microsoft.AspNetCore.Identity;

namespace Frelance.Infrastructure.Entities;

public class Users:IdentityUser<int>
{
    public Address Address { get; set; }
    public List<Projects> Projects { get; set; }
    public List<ProjectTasks> Tasks { get; set; }= [];
    public List<TimeLogs> TimeLogs { get; set; } = [];
    public List<Skiills> Skills { get; set; } = [];
    public List<string> ForeignLanguages { get; set; }
    public bool IsAvailable{get;set;}
}