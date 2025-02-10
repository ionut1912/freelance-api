namespace Frelance.Infrastructure.Entities;

public class Skiills
{
    public int Id{get;set;}
    public string ProgrammingLanguage { get; set; }
    public string Area{get;set;}
    public List<FreelancerProfiles> FreelancerProfiles { get; set; } = [];
}