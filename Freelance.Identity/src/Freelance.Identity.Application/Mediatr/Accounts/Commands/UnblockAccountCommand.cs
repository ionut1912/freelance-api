using Shared.Application.Mediator;

namespace Freelance.Identity.Application.Mediatr.Accounts.Commands;

public record UnblockAccountCommand(Guid AccountId) : IRequest<Unit>
{

}