namespace Freelance.Identity.Application.Dtos;

public sealed record AccountDto(
    Guid Id,
    string Email,
    string PhoneNumber,
    string Username,
    string? Token,
    string Role,
    bool IsBlocked,
    DateTime BlockedAt);
