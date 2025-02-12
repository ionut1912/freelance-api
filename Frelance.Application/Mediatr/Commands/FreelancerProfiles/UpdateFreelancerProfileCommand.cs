using Frelance.Contracts.Requests.Address;
using Frelance.Contracts.Requests.Skills;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Frelance.Application.Mediatr.Commands.FreelancerProfiles;

public record UpdateFreelancerProfileCommand(
    int Id,    
    AddressRequest Address,
    string Bio,
    IFormFile ProfileImage,
    List<SkillRequest> Skills,
    List<string> ForeignLanguages,
    string Experience,
    int Rate,
    string Currency,
    int Rating,
    string PortfolioUrl):IRequest<Unit>;