namespace Frelance.Contracts.Dtos;

public class UserProfileDto
{

    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<ReviewsDto> Reviews { get; set; }
    public List<ProposalsDto> Proposals { get; set; }
    public DateTime CreatedAt { get; set; }
}