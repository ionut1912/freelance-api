namespace Freelance.UserProfiles.Application.Dtos;

public sealed record ClientProfileDto(Guid Id, Guid AccountId, AddressDto Address, string Bio, string Image, bool IsVerified)
    : BaseProfileDto(AccountId, Address, Bio, Image, IsVerified);
