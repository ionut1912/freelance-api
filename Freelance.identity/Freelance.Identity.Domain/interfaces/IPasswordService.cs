namespace Freelance.Identity.Domain.interfaces;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashed);
}