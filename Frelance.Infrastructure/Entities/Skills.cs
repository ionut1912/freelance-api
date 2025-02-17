namespace Frelance.Infrastructure.Entities;

public class Skills
{
    public int Id { get; set; }
    public required string ProgrammingLanguage { get; set; }
    public required string Area { get; set; }
    public List<FreelancerProfiles> FreelancerProfiles { get; set; } = [];
}