using Freelancer.UserProfiles.Application.Dtos;

namespace Freelancer.UserProfiles.Application.Requests;

public class BaseCreateProfileRequest
{
    public AddressDto Address { get; set; }
    public string Bio { get; set; }
    public string Image { get; set; }
}