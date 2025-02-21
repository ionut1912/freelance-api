using Frelance.Contracts.Requests.ClientProfile;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.ClientProfiles;

public record CreateClientProfileCommand(CreateClientProfileRequest CreateClientProfileRequest) : IRequest<Unit>;