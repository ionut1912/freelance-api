using Freelancer.UserProfiles.Application.Dtos;

namespace Freelancer.UserProfiles.Application.Requests;

public class UpdateProfileAddressRequest
{
    public AddressDto AddressDto { get; set; }
}