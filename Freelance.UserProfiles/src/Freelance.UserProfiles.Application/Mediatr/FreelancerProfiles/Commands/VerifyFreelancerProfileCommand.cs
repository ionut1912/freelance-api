using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public record VerifyFreelancerProfileCommand(Guid AccountId, string ImageUrl):IRequest<Unit>
{
}
