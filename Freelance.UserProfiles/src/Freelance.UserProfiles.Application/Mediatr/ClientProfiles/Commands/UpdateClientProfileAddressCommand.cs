using Freelance.UserProfiles.Application.Dtos;
using Shared.Application.Mediator;
namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

public record UpdateClientProfileAddressCommand(Guid Id, AddressDto AddressDto) : IRequest<Unit>
{
}