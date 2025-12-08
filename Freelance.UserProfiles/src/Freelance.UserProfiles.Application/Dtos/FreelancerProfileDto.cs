namespace Freelance.UserProfiles.Application.Dtos;

public record FreelancerProfileDto(Guid AccountId,
    AddressDto Address, 
    string Bio,
    string Image, 
    bool IsVerified,
    string Experience,
    decimal Amount, 
    string Currency,
    int?Rating,
    string PortfolioUrl,
    List<string>ForeignLanguages,
    List<string> ProgrammingLanguages,
    List<string>Areas) : BaseProfileDto(AccountId,Address,Bio,Image,IsVerified)
{

}