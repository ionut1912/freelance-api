using Freelance.Identity.Application.Dtos;
using Freelance.Identity.Application.Mappings;
using Freelance.Identity.Application.Mediatr.Accounts.Query;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Microsoft.Extensions.Logging;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public sealed class LoginQueryHandler(
    IAccountRepository accountRepository,
    IPasswordService passwordService,
    IJwtTokenService jwtTokenService,
    ILogger<LoginQueryHandler> logger)
{
    public async Task<AccountDto> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAccountByUsernameAsync(query.Username, cancellationToken)
                      ?? throw new AccountNotFoundException($"Account with {query.Username} not found");

        if (!passwordService.VerifyPassword(query.Password, account.Password))
            throw new PasswordNotMatchException("Passwords do not match");

        if (account.IsBlocked)
            throw new AccountBlockedException("Account is blocked. Try again later");

        logger.LogInformation("Account {AccountId} logged in", account.Id);
        return account.ToDto(jwtTokenService.GenerateToken(account));
    }
}
