using Freelancer.UserProfiles.Application.Dtos;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

public class UpdateClientProfileAddressCommand:IRequest<Unit>
{
    public Guid Id { get; set; }
    public AddressDto AddressDto { get; set; }
}