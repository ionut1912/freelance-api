using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public record DeleteFreelancerProfileCommand(Guid Id) : IRequest<Unit>
{

}