namespace Frelance.Infrastructure.Entities
{
    public class Skills
    {
        public int Id { get; init; }
        public required string ProgrammingLanguage { get; init; }
        public required string Area { get; init; }
        
        public List<FreelancerProfiles> FreelancerProfiles { get; set; } = [];
    }
}