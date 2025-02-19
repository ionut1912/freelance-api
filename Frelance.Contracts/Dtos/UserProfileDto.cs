namespace Frelance.Contracts.Dtos;

public class UserProfileDto
{

    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required List<ReviewsDto> Reviews { get; set; }
    public required List<ProposalsDto> Proposals { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string Image { get; set; }
}