using Freelance.UserProfiles.Application.Dtos;
using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public record UpdateFreelancerProfileAddressCommand(Guid Id, AddressDto AddressDto) : IRequest<Unit>
{

}