using AutoMapper;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelancer.UserProfiles.Application.Dtos;
using Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class GetFreelancerProfilesQueryHandler : IRequestHandler<GetFreelancerProfilesQuery, List<FreelancerProfileDto>>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly IMapper _mapper;

    public GetFreelancerProfilesQueryHandler(IFreelancerProfileRepository freelancerProfileRepository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        _freelancerProfileRepository = freelancerProfileRepository;
        _mapper = mapper;
    }

    public async Task<List<FreelancerProfileDto>> Handle(GetFreelancerProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var freelancerProfiles = await _freelancerProfileRepository.GetFreelancerProfilesAsync(cancellationToken);
        var freelancerProfileDtos = _mapper.Map<List<FreelancerProfileDto>>(freelancerProfiles);
        return freelancerProfileDtos;
    }
}