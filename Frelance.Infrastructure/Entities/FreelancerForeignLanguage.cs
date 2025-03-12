namespace Frelance.Infrastructure.Entities;

public class FreelancerForeignLanguage
{
    public int Id { get; set; }
    public required string Language { get; init; }
    public int FreelancerProfileId { get; set; }
    public FreelancerProfiles? FreelancerProfile { get; init; }
}