using Frelance.Application.Mediatr.Queries.FreelancerProfiles;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.FreelancerProfiles;

public class
    GetFreelancerProfilesQueryHandler : IRequestHandler<GetFreelancerProfilesQuery, PaginatedList<FreelancerProfileDto>>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    public GetFreelancerProfilesQueryHandler(IFreelancerProfileRepository freelancerProfileRepository)
    {
        _freelancerProfileRepository = freelancerProfileRepository;
    }

    public async Task<PaginatedList<FreelancerProfileDto>> Handle(GetFreelancerProfilesQuery request,
        CancellationToken cancellationToken)
    {
        return await _freelancerProfileRepository.GetAllFreelancerProfilesAsync(request, cancellationToken);
    }
}