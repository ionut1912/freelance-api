namespace Freelance.Identity.Application.Mediatr.Accounts.Commands;

public sealed record CreateAccountCommand(string Email, string Password, string PhoneNumber, string Username, string Role);
