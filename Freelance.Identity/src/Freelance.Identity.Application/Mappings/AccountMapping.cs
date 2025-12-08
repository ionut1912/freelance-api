using Freelance.Identity.Application.Dtos;
using Freelance.Identity.Domain.Entities;

namespace Freelance.Identity.Application.Mappings
{
    public static class AccountMapping
    {
        public static AccountDto ToDto(this Account account)
        {
            return new AccountDto
            {
                Email = account.Email,
                PhoneNumber= account.PhoneNumber,
                Username = account.Username,
                Role=account.Role.Value,
                IsBlocked=account.IsBlocked,
                BlockedAt=account.BlockedAt,
            };
        }
    }
}
