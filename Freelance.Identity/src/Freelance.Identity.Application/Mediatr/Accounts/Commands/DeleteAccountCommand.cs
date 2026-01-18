using Shared.Application.Mediator;

namespace Freelance.Identity.Application.Mediatr.Accounts.Commands;

public record DeleteAccountCommand(Guid Id) : IRequest<Unit>
{

}