using Shared.Domain.Common;

namespace Freelance.UserProfiles.Domain.Entities;

public sealed class Skill : Entity
{
    private Skill() // for EF Core
    {
    }

    private Skill(string programmingLanguage, string area)
    {
        ProgrammingLanguage = programmingLanguage;
        Area = area;
    }

    public string ProgrammingLanguage { get; private set; } = string.Empty;
    public string Area { get; private set; } = string.Empty;

    public static Skill Create(string programmingLanguage, string area) => new(programmingLanguage, area);
}
