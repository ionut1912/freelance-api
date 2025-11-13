using AutoMapper;
using Freelance.Identity.Application.Dtos;
using Freelance.Identity.Application.Mediatr.Accounts.Query;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using MediatR;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public class LoginQueryHandler : IRequestHandler<LoginQuery, AccountDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;

    public LoginQueryHandler(IAccountRepository accountRepository, IJwtTokenService jwtTokenService, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(accountRepository);
        ArgumentNullException.ThrowIfNull(jwtTokenService);
        ArgumentNullException.ThrowIfNull(mapper);
        _accountRepository = accountRepository;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.LoginAsync(request.Username, request.Password);
        if (account.IsBlocked)
        {
            throw new AccountBlockedException("Acount is blocked.Try again later");
        }
        
        var accountDto = _mapper.Map<AccountDto>(account);
        accountDto.Token = _jwtTokenService.GenerateToken(account);
        return accountDto;
    }
}