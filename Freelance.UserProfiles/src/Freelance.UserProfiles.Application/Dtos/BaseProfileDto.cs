namespace Freelance.UserProfiles.Application.Dtos;

public record BaseProfileDto(Guid AccountId, AddressDto Address, string Bio, string Image, bool IsVerified)
{

}