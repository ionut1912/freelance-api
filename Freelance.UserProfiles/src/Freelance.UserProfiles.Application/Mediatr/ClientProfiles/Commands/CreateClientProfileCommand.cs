using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Domain.Entities;
using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

public record CreateClientProfileCommand(Guid AccountId,AddressDto AddressDto,string Bio,string Image) : IRequest<ClientProfile>
{
}