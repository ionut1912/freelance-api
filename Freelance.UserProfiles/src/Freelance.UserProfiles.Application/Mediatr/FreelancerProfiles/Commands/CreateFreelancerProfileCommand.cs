using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Domain.Entities;
using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public record CreateFreelancerProfileCommand(Guid AccountId,
    AddressDto Address,
    string Bio,
    string Image,
    string Experience,
    decimal Amount,
    string Currency,
    string PortfolioUrl,
    List<string> ForeignLanguages,
    List<string>ProgrammingLanguages,
    List<string>Areas) : IRequest<FreelancerProfile>
{
 
}