using Freelance.Identity.Application.Dtos;
using Freelance.Identity.Application.Mappings;
using Freelance.Identity.Application.Mediatr.Accounts.Query;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;

namespace Freelance.Identity.Application.Mediatr.Accounts.Handlers;

public sealed class GetCurrentAccountQueryHandler(IAccountRepository accountRepository)
{
    public async Task<AccountDto> Handle(GetCurrentAccountQuery query, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetAccountByUsernameAsync(query.Username, cancellationToken)
                      ?? throw new AccountNotFoundException($"Account with username '{query.Username}' was not found.");

        return account.ToDto();
    }
}
