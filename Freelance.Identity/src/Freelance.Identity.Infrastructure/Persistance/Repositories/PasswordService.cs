using Freelance.Identity.Domain.interfaces;

namespace Freelance.Identity.Infrastructure.Persistance.Repositories;

public sealed class PasswordService : IPasswordService
{
    // BCrypt work factor; 12 is a reasonable secure default.
    private const int WorkFactor = 12;

    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);

    public bool VerifyPassword(string password, string hashed) => BCrypt.Net.BCrypt.Verify(password, hashed);
}
