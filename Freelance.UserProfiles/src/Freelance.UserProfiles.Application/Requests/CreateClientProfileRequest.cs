using Freelance.UserProfiles.Application.Dtos;

namespace Freelance.UserProfiles.Application.Requests;

public record CreateClientProfileRequest(AddressDto Address, string Bio, string Image) : BaseCreateProfileRequest(Address,Bio,Image)
{
}