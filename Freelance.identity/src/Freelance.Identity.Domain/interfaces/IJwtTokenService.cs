using Freelance.Identity.Domain.Entities;

namespace Freelance.Identity.Domain.interfaces;

public interface IJwtTokenService
{
    string GenerateToken(Account account);
}