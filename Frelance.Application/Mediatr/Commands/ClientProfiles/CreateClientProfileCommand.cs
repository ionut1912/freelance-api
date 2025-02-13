using Frelance.Contracts.Requests.Address;
using Frelance.Contracts.Requests.ClientProfile;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Frelance.Application.Mediatr.Commands.ClientProfiles
{
    public record CreateClientProfileCommand(CreateClientProfileRequest CreateClientProfileRequest) : IRequest<Unit>;
}