using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;

public record VerifyClientProfileCommand(Guid AccountId,string ImageUrl) : IRequest<Unit>
{

}
