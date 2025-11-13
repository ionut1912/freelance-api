using Freelance.Shared.Domain.Common;

namespace Freelance.UserProfiles.Domain.Entities;

public class Skill:Entity
{
    public string ProgrammingLanguage { get; private set; }
    public string Area{get; private set;}
    

    private Skill()
    {
        
    }

    private Skill(string programmingLanguage, string area)
    {
        ProgrammingLanguage = programmingLanguage;
        Area = area;
        CreatedAt = DateTime.UtcNow;
    }

    public static Skill Create(string programmingLanguage, string area)
    {
        return new Skill(programmingLanguage, area);
    }
    
}