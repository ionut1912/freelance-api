using Freelance.Identity.Application.Dtos;
using Freelance.Identity.Application.Mappings;
using Freelance.Identity.Application.Mediatr.Accounts.Query;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Shared.Application.Mediator;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public class LoginQueryHandler : IRequestHandler<LoginQuery, AccountDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginQueryHandler(IAccountRepository accountRepository, IPasswordService passwordService, IJwtTokenService jwtTokenService)
    {
        ArgumentNullException.ThrowIfNull(accountRepository, nameof(accountRepository));
        ArgumentNullException.ThrowIfNull(passwordService, nameof(passwordService));
        ArgumentNullException.ThrowIfNull(jwtTokenService, nameof(jwtTokenService));
        _accountRepository = accountRepository;
        _passwordService = passwordService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AccountDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAccountByUsernameAsync(request.Username, cancellationToken);

        if (account is null)
        {
            throw new AccountNotFoundException($"Account with {request.Username} not found");
        }

        var isPasswordValid = _passwordService.VerifyPassword(request.Password, account.Password);

        if (!isPasswordValid)
        {
            throw new PasswordNotMatchException("Passwords do not match");

        }

        if (account.IsBlocked) throw new AccountBlockedException("Acount is blocked.Try again later");

        var accountDto = account.ToDto(_jwtTokenService.GenerateToken(account));
        return accountDto;
    }
}