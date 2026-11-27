using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;
using Freelance.UserProfiles.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public sealed class GetFreelancerProfilesQueryHandler(IFreelancerProfileRepository freelancerProfileRepository)
{
    public async Task<List<FreelancerProfileDto>> Handle(GetFreelancerProfilesQuery query,
        CancellationToken cancellationToken)
    {
        var profiles = await freelancerProfileRepository.GetAllAsync(cancellationToken, p => p.Skills,
            p => p.ForeignLanguages);
        return profiles.ToDtos();
    }
}
