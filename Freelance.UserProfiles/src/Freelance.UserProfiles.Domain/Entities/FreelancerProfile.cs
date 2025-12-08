using Freelance.UserProfiles.Domain.ValueObjects;

namespace Freelance.UserProfiles.Domain.Entities;

public class FreelancerProfile : BaseUserProfile
{
    // Internal collections
    private readonly List<FreelancerForeignLanguage> _foreignLanguages = new();
    private readonly List<Skill> _skills = new();

    // Private constructor for EF
    private FreelancerProfile()
    {
    }

    private FreelancerProfile(
        Guid accountId,
        string street,
        string city,
        string state,
        string zipCode,
        string country,
        string streetNumber,
        string bio,
        string image,
        string experience,
        decimal amount,
        string currency,
        int? rating,
        string portfolioUrl
    ) : base(accountId,street, city, state, zipCode, country, streetNumber, bio, image)
    {
        Experience = experience;
        Rate = Money.Create(amount, currency);
        Rating = rating ?? 0;
        PortfolioUrl = portfolioUrl;
    }

    // Exposed read-only collections for EF and domain logic
    public IReadOnlyCollection<FreelancerForeignLanguage> ForeignLanguages => _foreignLanguages.AsReadOnly();
    public IReadOnlyCollection<Skill> Skills => _skills.AsReadOnly();

    // Other properties
    public string Experience { get; private set; } = string.Empty;
    public Money Rate { get; private set; }=Money.Create(0, "USD");
    public int? Rating { get; private set; } = 0;
    public string PortfolioUrl { get; private set; } = string.Empty;

    // Factory method
    public static FreelancerProfile Create(
        Guid accountId,
        string street,
        string city,
        string state,
        string zipCode,
        string country,
        string streetNumber,
        string bio,
        string image,
        string experience,
        decimal amount,
        string currency,
        int? rating,
        string portfolioUrl
    )
    {
        return new FreelancerProfile(accountId, street, city, state, zipCode, country, streetNumber, bio, image,
            experience, amount, currency, rating, portfolioUrl);
    }

    // Methods to manage collections
    public void AddLanguages(List<FreelancerForeignLanguage> languages)
    {
        _foreignLanguages.AddRange(languages);
        CreatedAt = DateTime.UtcNow;
    }

    public void AddSkills(List<Skill> skills)
    {
        _skills.AddRange(skills);
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateRating(int newRating)
    {
        Rating = newRating;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateLanguages(List<FreelancerForeignLanguage> languages)
    {
        _foreignLanguages.Clear();
        _foreignLanguages.AddRange(languages);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateSkills(List<Skill> skills)
    {
        _skills.Clear();
        _skills.AddRange(skills);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateFreelancerDetails(string newExperience, decimal newAmount, string newCurrency,
        string newPortfolioUrl)
    {
        Experience = newExperience;
        Rate = Money.Create(newAmount, newCurrency);
        PortfolioUrl = newPortfolioUrl;
        UpdatedAt = DateTime.UtcNow;
    }
}