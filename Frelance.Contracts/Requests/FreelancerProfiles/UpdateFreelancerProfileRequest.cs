using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.FreelancerProfiles;

[UsedImplicitly]
public class UpdateFreelancerProfileRequest(
    string addressCountry,
    string addressStreet,
    string addressStreetNumber,
    string addressCity,
    string addressZip,
    string bio,
    List<string> programmingLanguages,
    List<string> areas,
    List<string> foreignLanguages,
    string experience,
    int rate,
    string currency,
    int rating,
    string portfolioUrl,
    string image)
{
    public string? AddressCountry { get; } = addressCountry;
    public string? AddressStreet { get; } = addressStreet;
    public string? AddressStreetNumber { get; } = addressStreetNumber;
    public string? AddressCity { get; } = addressCity;
    public string? AddressZip { get; } = addressZip;
    public string? Bio { get; } = bio;
    public List<string>? ProgrammingLanguages { get; } = programmingLanguages;
    public List<string>? Areas { get; } = areas;
    public List<string>? ForeignLanguages { get; } = foreignLanguages;
    public string? Experience { get; } = experience;
    public int Rate { get; } = rate;
    public string? Currency { get; } = currency;
    public int Rating { get; } = rating;
    public string? PortfolioUrl { get; } = portfolioUrl;
    public string? Image { get; } = image;
}