using Shared.Application.Mediator;

namespace Freelance.Identity.Application.Mediatr.Accounts.Commands;

public class DeleteAccountCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}