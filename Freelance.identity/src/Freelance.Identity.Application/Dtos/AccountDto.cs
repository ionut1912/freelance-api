namespace Freelance.Identity.Application.Dtos;

public record AccountDto(string Email,
    string PhoneNumber,
    string Username, 
    string? Token, 
    string Role,
    bool IsBlocked,
    DateTime BlockedAt)
{
}