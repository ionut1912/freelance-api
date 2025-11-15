using Freelance.UserProfiles.Domain.Entities;
using Freelancer.UserProfiles.Application.Dtos;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public class CreateFreelancerProfileCommand : IRequest<FreelancerProfile>
{
    public Guid AccountId { get; set; }
    public AddressDto Address { get; set; }
    public string Bio { get; set; }
    public string Image { get; set; }
    public string Experience { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string PortfolioUrl { get; set; }
    public List<string> ForeignLanguages { get; set; }
    public List<string> ProgrammingLanguages { get; set; }
    public List<string> Areas { get; set; }
}