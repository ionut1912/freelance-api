using Freelance.UserProfiles.Application.Dtos;

namespace Freelance.UserProfiles.Application.Requests;

public record CreateFreelancerProfileRequest(AddressDto Address,
    string Bio,
    string Image,
    string Experience,
    decimal Amount,
    string Currency,
    string PortfolioUrl,
    List<string> ForeignLanguages,
    List<string> ProgrammingLanguages,
    List<string> Areas) : BaseCreateProfileRequest(Address, Bio, Image)
{

}