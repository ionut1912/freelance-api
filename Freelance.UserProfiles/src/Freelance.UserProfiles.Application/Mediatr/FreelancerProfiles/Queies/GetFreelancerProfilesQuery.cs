using Freelance.UserProfiles.Application.Dtos;
using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;

public record GetFreelancerProfilesQuery : IRequest<List<FreelancerProfileDto>>
{
}