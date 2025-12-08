
using Shared.Domain.Common;

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

    public string ProgrammingLanguage { get; private set; }= string.Empty;
    public string Area { get; private set; } = string.Empty;

    public static Skill Create(string programmingLanguage, string area)
    {
        return new Skill(programmingLanguage, area);
    }
}