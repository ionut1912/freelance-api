using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class GetLoggedInFreelancerProfileQueryHandler : IRequestHandler<GetLoggedInFreelancerProfileQuery, FreelancerProfileDto>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    public GetLoggedInFreelancerProfileQueryHandler(IFreelancerProfileRepository freelancerProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        _freelancerProfileRepository = freelancerProfileRepository;
    }

    public async Task<FreelancerProfileDto> Handle(GetLoggedInFreelancerProfileQuery request,
        CancellationToken cancellationToken)
    {
        var freelancerProfile =
            await _freelancerProfileRepository.GetLoggedInFreelancerProfileAsync(request.AccountId, cancellationToken);
        if (freelancerProfile == null)
            throw new ProfileNotFoundException($"Freelancer Profile with AccountId {request.AccountId} not found");
        var freelancerProfileDto = freelancerProfile.ToDto();
        return freelancerProfileDto;
    }
}