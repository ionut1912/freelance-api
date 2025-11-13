using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Domain.ValueObjects;
using Freelance.Shared.Domain.Common;

namespace Freelance.Identity.Domain.Entities;

public class Account : Entity
{
    private Account() //for EF core
    {
    }

    private Account(string email, string password, string phoneNumber, string username, Role role)
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentNullException("Email cannot be null or empty");
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException("Password cannot be null or empty");
        if (string.IsNullOrEmpty(phoneNumber))
            throw new ArgumentNullException("Phone Number cannot be null or empty");
        if (string.IsNullOrEmpty(username))
            throw new ArgumentNullException("Username cannot be null or empty");
        if (string.IsNullOrEmpty(role.Value))
            throw new ArgumentNullException("Role cannot be null or empty");
        
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
        Username = username;
        Role = role;
        IsBlocked = false;
        CreatedAt = DateTime.UtcNow;
    }

    public string Email { get; private set; }
    public string Password { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Username { get; private set; }
    public Role Role { get; private set; }
    public bool IsBlocked { get; private set; }
    public DateTime BlockedAt { get; private set; }

    public static Account Create(string email, string password, string phoneNumber, string username, Role role)
    {
        return new Account(email, password, phoneNumber, username, role);
    }

    public void BlockAccount()
    {
        if (IsBlocked)
            throw new AccountAlreadyBlockedException("Account is blocked");

        IsBlocked = true;
        UpdatedAt = DateTime.UtcNow;
        BlockedAt = DateTime.UtcNow;
    }
    
    public void HashPassword(IPasswordService passwordService)
    {
        if (string.IsNullOrWhiteSpace(Password))
            throw new InvalidOperationException("Password cannot be empty before hashing.");

        Password = passwordService.HashPassword(Password);
    }
    public void UnblockAccount()
    {
        if (!IsBlocked) throw new AccountNotBlockedException("Account is not blocked");

        IsBlocked = false;
        UpdatedAt = DateTime.UtcNow;
    }
}