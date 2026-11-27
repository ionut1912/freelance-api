using Freelance.Identity.Application.Dtos;
using Freelance.Identity.Domain.Entities;

namespace Freelance.Identity.Application.Mappings;

public static class AccountMapping
{
    extension(Account account)
    {
        public AccountDto ToDto(string? token = null) =>
            new(account.Id, account.Email, account.PhoneNumber, account.Username, token, account.Role.Value,
                account.IsBlocked, account.BlockedAt);
    }
}
