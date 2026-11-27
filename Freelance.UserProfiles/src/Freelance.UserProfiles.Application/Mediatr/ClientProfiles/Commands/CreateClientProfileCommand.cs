using Freelance.UserProfiles.Application.Dtos;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

public sealed record CreateClientProfileCommand(Guid AccountId, AddressDto AddressDto, string Bio, string Image);
