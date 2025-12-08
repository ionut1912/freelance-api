using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

public record UpdateFreelancerProfileDataCommand(Guid Id, string Bio,string Image) : IRequest<Unit>
{

}