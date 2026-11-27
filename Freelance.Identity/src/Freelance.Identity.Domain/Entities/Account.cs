using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Domain.ValueObjects;
using Shared.Domain.Common;

namespace Freelance.Identity.Domain.Entities;

public sealed class Account : Entity
{
    private Account() //for EF core
    {
    }

    private Account(string email, string password, string phoneNumber, string username, Role role)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);
        ArgumentException.ThrowIfNullOrWhiteSpace(phoneNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        ArgumentNullException.ThrowIfNull(role);

        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
        Username = username;
        Role = role;
        IsBlocked = false;
    }

    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public Role Role { get; private set; } = Role.Freelancer;
    public bool IsBlocked { get; private set; }
    public DateTime BlockedAt { get; private set; }

    public static Account Create(string email, string password, string phoneNumber, string username, Role role) =>
        new(email, password, phoneNumber, username, role);

    public void BlockAccount()
    {
        if (IsBlocked)
            throw new AccountAlreadyBlockedException("Account is blocked");

        IsBlocked = true;
        UpdatedAt = BlockedAt = DateTime.UtcNow;
    }

    public void HashPassword(IPasswordService passwordService)
    {
        ArgumentNullException.ThrowIfNull(passwordService);
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
