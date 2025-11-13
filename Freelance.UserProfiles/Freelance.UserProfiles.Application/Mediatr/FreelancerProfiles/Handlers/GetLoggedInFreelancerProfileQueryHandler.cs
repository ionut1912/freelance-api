using AutoMapper;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelancer.UserProfiles.Application.Dtos;
using Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class GetLoggedInFreelancerProfileQueryHandler:IRequestHandler<GetLoggedInFreelancerProfileQuery,FreelancerProfileDto>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IMapper _mapper;

    public GetLoggedInFreelancerProfileQueryHandler(IFreelancerProfileRepository freelancerProfileRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        _freelancerProfileRepository = freelancerProfileRepository;
        _mapper = mapper;
    }
    
    public async Task<FreelancerProfileDto> Handle(GetLoggedInFreelancerProfileQuery request, CancellationToken cancellationToken)
    {
        var freelancerProfile =
            await _freelancerProfileRepository.GetLoggedInFreelancerProfileAsync(request.AccountId, cancellationToken);
        if (freelancerProfile == null)
            throw new ProfileNotFoundException($"Freelancer Profile with AccountId {request.AccountId} not found");
        var freelancerProfileDto= _mapper.Map<FreelancerProfileDto>(freelancerProfile);
        return freelancerProfileDto;
    }
}