namespace Frelance.Infrastructure.Entities;

public class FreelancerForeignLanguage
{
    public int Id { get; set; }
    public string Language { get; set; }
    public int FreelancerProfileId { get; set; }
    public FreelancerProfiles FreelancerProfile { get; set; }
}
