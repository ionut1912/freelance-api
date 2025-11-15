using Freelance.Shared.Domain.Common;

namespace Freelance.UserProfiles.Domain.Entities;

public class Skill : Entity
{
    private Skill()
    {
    }

    private Skill(string programmingLanguage, string area)
    {
        ProgrammingLanguage = programmingLanguage;
        Area = area;
        CreatedAt = DateTime.UtcNow;
    }

    public string ProgrammingLanguage { get; private set; }
    public string Area { get; private set; }

    public static Skill Create(string programmingLanguage, string area)
    {
        return new Skill(programmingLanguage, area);
    }
}