using Frelance.Contracts.Requests.ClientProfile;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.ClientProfiles;

public record UpdateClientProfileCommand(
    int Id,
    UpdateClientProfileRequest UpdateClientProfileRequest) : IRequest<Unit>;