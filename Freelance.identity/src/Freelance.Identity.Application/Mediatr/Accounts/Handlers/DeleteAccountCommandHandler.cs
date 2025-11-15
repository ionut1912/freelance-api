using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.interfaces;
using MediatR;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Unit>
{
    private readonly IAccountRepository _accountRepository;

    public DeleteAccountCommandHandler(IAccountRepository accountRepository)
    {
        ArgumentNullException.ThrowIfNull(accountRepository, nameof(accountRepository));
        _accountRepository = accountRepository;
    }

    public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAccountAsync(request.Id);
        await _accountRepository.DeleteAsync(account, cancellationToken);
        return Unit.Value;
    }
}