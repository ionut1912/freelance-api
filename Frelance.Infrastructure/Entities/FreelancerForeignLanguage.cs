namespace Frelance.Infrastructure.Entities;

public class FreelancerForeignLanguage:BaseEntity
{
    public int Id { get; init; }
    public required string Language { get; init; }
    public int FreelancerProfileId { get; init; }
    public FreelancerProfiles? FreelancerProfile { get; init; }
}