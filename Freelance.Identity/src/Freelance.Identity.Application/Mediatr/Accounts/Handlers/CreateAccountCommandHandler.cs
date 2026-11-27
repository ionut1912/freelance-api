using Freelance.Identity.Application.Dtos;
using Freelance.Identity.Application.Mappings;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public sealed class CreateAccountCommandHandler(
    IAccountRepository accountRepository,
    IPasswordService passwordService,
    IUnitOfWork unitOfWork,
    ILogger<CreateAccountCommandHandler> logger)
{
    public async Task<AccountDto> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        if (await accountRepository.GetAccountByUsernameAsync(command.Username, cancellationToken) is not null)
            throw new UserAlreadyExistsException($"Account with username {command.Username} already exists");

        var account = Account.Create(command.Email, command.Password, command.PhoneNumber, command.Username,
            Role.FromName(command.Role));
        account.HashPassword(passwordService);

        await accountRepository.AddAsync(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Account {AccountId} was created", account.Id);
        return account.ToDto();
    }
}
