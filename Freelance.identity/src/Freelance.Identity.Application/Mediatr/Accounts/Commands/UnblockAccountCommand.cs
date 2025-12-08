using Shared.Application.Mediator;

namespace Freelance.Identity.Application.Mediatr.Accounts.Commands;

public class UnblockAccountCommand : IRequest<Unit>
{
    public Guid AccountId { get; set; }
}