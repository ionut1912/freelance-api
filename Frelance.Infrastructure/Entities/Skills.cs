namespace Frelance.Infrastructure.Entities;

public class Skills
{
    public int Id { get; set; }
    public string ProgrammingLanguage { get; set; }
    public string Area { get; set; }
    public List<FreelancerProfiles> FreelancerProfiles { get; set; } = [];
}