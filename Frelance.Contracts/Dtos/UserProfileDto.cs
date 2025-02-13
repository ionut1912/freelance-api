namespace Frelance.Contracts.Dtos;

public record UserProfileDto(int Id, string Username, string Email, string PhoneNumber, List<ReviewsDto> Reviews, List<ProposalsDto> Proposals);