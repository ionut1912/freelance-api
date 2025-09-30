using JetBrains.Annotations;

namespace Freelance.Contracts.Dtos;

[UsedImplicitly]
public record UserDto(string PhoneNumber, string Token, string Username, string Email, DateTime CreatedAt);