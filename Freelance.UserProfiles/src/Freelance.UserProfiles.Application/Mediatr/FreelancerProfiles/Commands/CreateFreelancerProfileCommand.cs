using Freelance.UserProfiles.Application.Dtos;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public sealed record CreateFreelancerProfileCommand(
    Guid AccountId,
    AddressDto Address,
    string Bio,
    string Image,
    string Experience,
    decimal Amount,
    string Currency,
    string PortfolioUrl,
    List<string> ForeignLanguages,
    List<string> ProgrammingLanguages,
    List<string> Areas);
