using Freelancer.UserProfiles.Application.Dtos;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;

public class GetLoggedInFreelancerProfileQuery : IRequest<FreelancerProfileDto>
{
    public Guid AccountId { get; set; }
}