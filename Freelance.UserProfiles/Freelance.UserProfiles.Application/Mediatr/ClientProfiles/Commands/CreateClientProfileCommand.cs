using Freelance.UserProfiles.Domain.Entities;
using Freelancer.UserProfiles.Application.Dtos;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

public class CreateClientProfileCommand:IRequest<ClientProfile>
{
    public Guid AccountId { get; set; }
    public AddressDto Address { get; set; }
    public string Bio { get; set; }
    public string Image { get; set; }
}