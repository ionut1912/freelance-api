using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

public record DeleteClientProfileCommand(Guid Id) : IRequest<Unit>
{
}