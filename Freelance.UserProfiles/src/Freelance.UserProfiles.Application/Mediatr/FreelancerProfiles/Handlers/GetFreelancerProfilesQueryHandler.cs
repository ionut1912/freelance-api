using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;
using Freelance.UserProfiles.Domain.Interfaces;
using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class GetFreelancerProfilesQueryHandler : IRequestHandler<GetFreelancerProfilesQuery, List<FreelancerProfileDto>>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    public GetFreelancerProfilesQueryHandler(IFreelancerProfileRepository freelancerProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        _freelancerProfileRepository = freelancerProfileRepository;
    }

    public async Task<List<FreelancerProfileDto>> Handle(GetFreelancerProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var freelancerProfiles = await _freelancerProfileRepository.GetFreelancerProfilesAsync(cancellationToken);
        var freelancerProfileDtos = freelancerProfiles.ToDtos();
        return freelancerProfileDtos;
    }
}