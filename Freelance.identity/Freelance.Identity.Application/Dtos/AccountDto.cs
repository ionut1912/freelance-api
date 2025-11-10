namespace Freelance.Identity.Application.Dtos;

public class AccountDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Username { get; set; }
    public AddressDto Address { get; set; }
    public string? Token { get; set; }
    public string Role { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime BlockedAt { get; set; }
}