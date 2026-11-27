using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public sealed class BlockAccountCommandHandler(
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork,
    ILogger<BlockAccountCommandHandler> logger)
{
    public async Task Handle(BlockAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByIdAsync(command.AccountId, cancellationToken)
                      ?? throw new AccountNotFoundException($"Account with id {command.AccountId} does not exist");

        account.BlockAccount();
        accountRepository.Update(account);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Account {AccountId} was blocked", command.AccountId);
    }
}
