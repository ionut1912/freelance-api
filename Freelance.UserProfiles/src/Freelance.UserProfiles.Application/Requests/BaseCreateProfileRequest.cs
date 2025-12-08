using Freelance.UserProfiles.Application.Dtos;

namespace Freelance.UserProfiles.Application.Requests;

public record BaseCreateProfileRequest(AddressDto Address, string Bio, string Image)
{

}