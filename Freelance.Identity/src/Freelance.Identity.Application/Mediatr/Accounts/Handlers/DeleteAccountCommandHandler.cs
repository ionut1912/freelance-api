using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public sealed class DeleteAccountCommandHandler(
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteAccountCommandHandler> logger)
{
    public async Task Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByIdAsync(command.Id, cancellationToken)
                      ?? throw new AccountNotFoundException($"Account with Id {command.Id} was not found.");

        accountRepository.Delete(account);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Account {AccountId} was deleted", command.Id);
    }
}
