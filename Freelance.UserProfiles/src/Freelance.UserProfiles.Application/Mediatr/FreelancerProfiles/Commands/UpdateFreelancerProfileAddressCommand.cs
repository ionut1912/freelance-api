using Freelance.UserProfiles.Application.Dtos;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public sealed record UpdateFreelancerProfileAddressCommand(Guid Id, AddressDto AddressDto);
