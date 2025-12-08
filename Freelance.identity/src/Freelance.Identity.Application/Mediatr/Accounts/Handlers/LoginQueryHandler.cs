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
    private readonly IJwtTokenService _jwtTokenService;

    public LoginQueryHandler(IAccountRepository accountRepository, IJwtTokenService jwtTokenService)
    {
        ArgumentNullException.ThrowIfNull(accountRepository);
        ArgumentNullException.ThrowIfNull(jwtTokenService);
        _accountRepository = accountRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AccountDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.LoginAsync(request.Username, request.Password);
        if (account.IsBlocked) throw new AccountBlockedException("Acount is blocked.Try again later");

        var accountDto = account.ToDto();
        accountDto.Token = _jwtTokenService.GenerateToken(account);
        return accountDto;
    }
}