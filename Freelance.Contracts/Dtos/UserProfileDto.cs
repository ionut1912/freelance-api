using JetBrains.Annotations;

namespace Freelance.Contracts.Dtos;

[UsedImplicitly]
public class UserProfileDto(
    int id,
    string username,
    string email,
    string phoneNumber,
    List<ReviewsDto> reviews,
    List<ProposalsDto> proposals,
    DateTime createdAt)
{
    public int Id { get; } = id;
    public required string Username { get; init; } = username;
    public required string Email { get; init; } = email;
    public required string PhoneNumber { get; init; } = phoneNumber;
    public required List<ReviewsDto> Reviews { get; init; } = reviews;
    public required List<ProposalsDto> Proposals { get; init; } = proposals;
    public DateTime CreatedAt { get; } = createdAt;
}