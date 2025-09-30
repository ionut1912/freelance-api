using JetBrains.Annotations;

namespace Frelance.Infrastructure.Entities;

public class Skills:BaseEntity
{
    public int Id { get; init; }
    public required string ProgrammingLanguage { get; init; }
    public required string Area { get; init; }

    [UsedImplicitly] public List<FreelancerProfileSkill> FreelancerProfileSkills { get; init; } = [];
}