using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using MediatR;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public class UnblockAccountCommandHandler : IRequestHandler<UnblockAccountCommand, Unit>
{
    private readonly IAccountRepository _accountRepository;

    public UnblockAccountCommandHandler(IAccountRepository accountRepository)
    {
        ArgumentNullException.ThrowIfNull(accountRepository, nameof(accountRepository));
        _accountRepository = accountRepository;
    }

    public async Task<Unit> Handle(UnblockAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAccountAsync(request.AccountId);
        if (account == null)
            throw new AccountNotFoundException($"Account with id {request.AccountId} does not exist");
        account.UnblockAccount();
        await _accountRepository.UnblockAccountAsync(account, cancellationToken);
        return Unit.Value;
    }
}