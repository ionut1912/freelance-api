using Frelance.Contracts.Requests.Address;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Frelance.Application.Mediatr.Commands.ClientProfiles;

public record UpdateClientProfileCommand(
    int Id,
    AddressRequest AddressRequest,
    string Bio,
    IFormFile ProfileImage) : IRequest<Unit>;