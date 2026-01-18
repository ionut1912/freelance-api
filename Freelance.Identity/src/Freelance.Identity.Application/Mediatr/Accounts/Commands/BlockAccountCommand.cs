using Shared.Application.Mediator;

namespace Freelance.Identity.Application.Mediatr.Accounts.Commands;

public record BlockAccountCommand(Guid AccountId) : IRequest<Unit>
{

}