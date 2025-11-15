namespace Freelancer.UserProfiles.Application.Dtos;

public class BaseProfileDto
{
    public Guid AccountId { get; set; }
    public AddressDto Address { get; set; }
    public string Bio { get; set; }
    public string Image { get; set; }
    public bool IsVerified { get; set; }
}