namespace Frelance.Contracts.Dtos;

public record UserClientDto(int Id, string Username, string Email, string PhoneNumber, List<ReviewsDto> Reviews, List<ProposalsDto> Proposals, List<ProjectDto> Projects);