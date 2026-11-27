using Freelance.UserProfiles.Application.Dtos;

namespace Freelance.UserProfiles.Application.Requests;

public sealed record CreateClientProfileRequest(AddressDto Address, string Bio, string Image) : BaseCreateProfileRequest(Address, Bio, Image)
{
}