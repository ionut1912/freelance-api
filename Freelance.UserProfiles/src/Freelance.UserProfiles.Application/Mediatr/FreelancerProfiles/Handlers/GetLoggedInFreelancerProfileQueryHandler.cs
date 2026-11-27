using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public sealed class GetLoggedInFreelancerProfileQueryHandler(IFreelancerProfileRepository freelancerProfileRepository)
{
    public async Task<FreelancerProfileDto> Handle(GetLoggedInFreelancerProfileQuery query,
        CancellationToken cancellationToken)
    {
        var profile = await freelancerProfileRepository.GetLoggedInFreelancerProfileAsync(query.AccountId, cancellationToken)
                      ?? throw new ProfileNotFoundException($"Freelancer Profile with AccountId {query.AccountId} not found");

        return profile.ToDto();
    }
}
