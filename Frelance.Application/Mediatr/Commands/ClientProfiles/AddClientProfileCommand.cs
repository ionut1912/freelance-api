// AddClientProfileCommand.cs

using Frelance.Contracts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Frelance.Application.Mediatr.Commands.ClientProfiles
{
    public record AddClientProfileCommand(AddressDto Address, string Bio, IFormFile ProfileImage) : IRequest<Unit>;
}