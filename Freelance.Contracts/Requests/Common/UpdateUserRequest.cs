namespace Freelance.Contracts.Requests.Common;

public record UpdateUserRequest(string Username,string Email,string PhoneNumber,string Bio);