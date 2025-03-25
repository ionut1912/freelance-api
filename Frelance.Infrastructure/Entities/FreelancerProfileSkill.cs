namespace Frelance.Infrastructure.Entities;

public class FreelancerProfileSkill
{
    public int FreelancerProfileId { get; init; }
    public FreelancerProfiles? FreelancerProfile { get; init; }
    public int SkillId { get; init; }
    public Skills? Skill { get; init; }
}