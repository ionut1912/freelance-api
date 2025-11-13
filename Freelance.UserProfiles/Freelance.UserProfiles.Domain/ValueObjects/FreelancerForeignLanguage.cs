using Freelance.Shared.Domain.Common;
using Freelance.UserProfiles.Domain.Entities;

namespace Freelance.UserProfiles.Domain.ValueObjects;

public class FreelancerForeignLanguage:ValueObject
{
    public string Language { get; private set; }
    

    private FreelancerForeignLanguage()
    {
        
    }

    private FreelancerForeignLanguage(string language)
    {
        Language = language;
    }

    public static FreelancerForeignLanguage Create(string language)
    {
        return new FreelancerForeignLanguage(language);
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Language;
    }
}