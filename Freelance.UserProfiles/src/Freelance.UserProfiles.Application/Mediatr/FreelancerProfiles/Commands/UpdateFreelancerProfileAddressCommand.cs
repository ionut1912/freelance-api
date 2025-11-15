using Freelancer.UserProfiles.Application.Dtos;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public class UpdateFreelancerProfileAddressCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public AddressDto AddressDto { get; set; }
}