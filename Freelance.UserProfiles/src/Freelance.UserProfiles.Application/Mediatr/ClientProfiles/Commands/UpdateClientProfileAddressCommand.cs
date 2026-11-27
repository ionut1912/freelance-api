using Freelance.UserProfiles.Application.Dtos;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

public sealed record UpdateClientProfileAddressCommand(Guid Id, AddressDto AddressDto, Guid AccountId);
