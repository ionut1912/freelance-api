using Frelance.Contracts.Requests.Address;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Frelance.Application.Mediatr.Commands.ClientProfiles
{
    public record AddClientProfileCommand(AddressRequest Address, string Bio, IFormFile ProfileImage) : IRequest<Unit>;
}