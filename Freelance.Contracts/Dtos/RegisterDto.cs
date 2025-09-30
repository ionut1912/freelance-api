using JetBrains.Annotations;

namespace Freelance.Contracts.Dtos;

[UsedImplicitly]
public record RegisterDto(string Email, string Password, string Username, string PhoneNumber, string Role);