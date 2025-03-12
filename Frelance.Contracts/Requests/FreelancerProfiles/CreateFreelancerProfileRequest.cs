using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.FreelancerProfiles;

[UsedImplicitly]
public class CreateFreelancerProfileRequest(
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
    public required string AddressCountry { get; init; } = addressCountry;
    public required string AddressStreet { get; init; } = addressStreet;
    public required string AddressStreetNumber { get; init; } = addressStreetNumber;
    public required string AddressCity { get; init; } = addressCity;
    public required string AddressZip { get; init; } = addressZip;
    public required string Bio { get; init; } = bio;

    public required List<string> ProgrammingLanguages { get; init; } = programmingLanguages;

    public required List<string> Areas { get; init; } = areas;

    public required List<string> ForeignLanguages { get; init; } = foreignLanguages;
    public required string Experience { get; init; } = experience;
    public int Rate { get; } = rate;
    public required string Currency { get; init; } = currency;
    public int Rating { get; } = rating;
    public required string PortfolioUrl { get; init; } = portfolioUrl;
    public required string Image { get; init; } = image;
}