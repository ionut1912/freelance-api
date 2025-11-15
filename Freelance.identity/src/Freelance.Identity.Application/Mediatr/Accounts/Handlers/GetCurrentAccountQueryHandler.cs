using AutoMapper;
using Freelance.Identity.Application.Dtos;
using Freelance.Identity.Application.Mediatr.Accounts.Query;
using Freelance.Identity.Domain.interfaces;
using MediatR;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public class GetCurrentAccountQueryHandler : IRequestHandler<GetCurrentAccountQuery, AccountDto>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public GetCurrentAccountQueryHandler(IAccountRepository accountRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(accountRepository);
        ArgumentNullException.ThrowIfNull(mapper);
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(GetCurrentAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetCurrentAccountAsync(request.Username);
        var accountDto = _mapper.Map<AccountDto>(account);
        return accountDto;
    }
}