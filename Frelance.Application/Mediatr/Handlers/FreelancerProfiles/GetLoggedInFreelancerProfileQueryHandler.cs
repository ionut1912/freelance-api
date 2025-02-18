using Frelance.Application.Mediatr.Queries.FreelancerProfiles;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.FreelancerProfiles;

public class GetLoggedInFreelancerProfileQueryHandler:IRequestHandler<GetLoggedInFreelancerProfileQuery,FreelancerProfileDto?>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    public GetLoggedInFreelancerProfileQueryHandler(IFreelancerProfileRepository freelancerProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        _freelancerProfileRepository = freelancerProfileRepository;
    }
    
    public async Task<FreelancerProfileDto?> Handle(GetLoggedInFreelancerProfileQuery request, CancellationToken cancellationToken)
    {
        return await _freelancerProfileRepository.GetLoggedInFreelancerProfileAsync(request, cancellationToken);
    }
}