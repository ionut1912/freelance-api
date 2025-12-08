using Freelance.Identity.Application.Dtos;
using Freelance.Identity.Application.Mappings;
using Freelance.Identity.Application.Mediatr.Accounts.Query;
using Freelance.Identity.Domain.interfaces;
using Shared.Application.Mediator;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public class GetCurrentAccountQueryHandler : IRequestHandler<GetCurrentAccountQuery, AccountDto>
{
    private readonly IAccountRepository _accountRepository;
    public GetCurrentAccountQueryHandler(IAccountRepository accountRepository)
    {
        ArgumentNullException.ThrowIfNull(accountRepository);
        _accountRepository = accountRepository;
    }

    public async Task<AccountDto> Handle(GetCurrentAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetCurrentAccountAsync(request.Username);
        var accountDto = account.ToDto(null);
        return accountDto;
    }
}