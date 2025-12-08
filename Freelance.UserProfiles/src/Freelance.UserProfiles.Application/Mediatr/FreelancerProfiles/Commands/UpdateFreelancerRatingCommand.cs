using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public record UpdateFreelancerRatingCommand(Guid Id, int Rating) : IRequest<Unit>
{
}