using Freelance.Identity.Application.Dtos;
using Freelance.Identity.Domain.Entities;

namespace Freelance.Identity.Application.Mappings;

public static class AccountMapping
{
    public static AccountDto ToDto(this Account account, string? token)
    {
        return new AccountDto(account.Email, account.PhoneNumber, account.Username, token, account.Role.Value, account.IsBlocked, account.BlockedAt);
    }
}
