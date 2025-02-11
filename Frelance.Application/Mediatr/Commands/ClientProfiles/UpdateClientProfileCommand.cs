using MediatR;
using Microsoft.AspNetCore.Http;

namespace Frelance.Application.Mediatr.Commands.ClientProfiles;

public record UpdateClientProfileCommand(
    int Id,
    string AddressCountry,
    string AddressStreet,
    string AddressStreetNumber,
    string AddressCity,
    string AddressZip,
    string Bio,
    IFormFile ProfileImage) : IRequest<Unit>;