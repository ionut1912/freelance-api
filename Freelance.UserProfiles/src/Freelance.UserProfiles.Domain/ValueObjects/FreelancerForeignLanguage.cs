using Shared.Domain.Common;

namespace Freelance.UserProfiles.Domain.ValueObjects;

public sealed class FreelancerForeignLanguage : ValueObject
{
    public const int MaxLength = 100;

    private FreelancerForeignLanguage()
    {
    }

    private FreelancerForeignLanguage(string language)
    {
        Language = language;
    }

    public string Language { get; } = string.Empty;

    public static FreelancerForeignLanguage Create(string language)
    {
        return new FreelancerForeignLanguage(language);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Language;
    }
}