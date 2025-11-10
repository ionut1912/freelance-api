using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using MediatR;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public class BlockAccountCommandHandler : IRequestHandler<BlockAccountCommand, Unit>
{
    private readonly IAccountRepository _accountRepository;

    public BlockAccountCommandHandler(IAccountRepository accountRepository)
    {
        ArgumentNullException.ThrowIfNull(accountRepository);
        _accountRepository = accountRepository;
    }

    public async Task<Unit> Handle(BlockAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAccountAsync(request.AccountId);
        if (account == null)
            throw new AccountNotFoundException($"Account with id {request.AccountId} does not exist");
        account.BlockAccount();
        await _accountRepository.BlockAccountAsync(account,cancellationToken);
        return Unit.Value;
    }
}