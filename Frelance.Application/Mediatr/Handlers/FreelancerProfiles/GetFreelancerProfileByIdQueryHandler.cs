using Frelance.Application.Mediatr.Queries.FreelancerProfiles;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.FreelancerProfiles;

public class GetFreelancerProfileByIdQueryHandler : IRequestHandler<GetFreelancerProfileByIdQuery, FreelancerProfileDto>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    public GetFreelancerProfileByIdQueryHandler(IFreelancerProfileRepository freelancerProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        _freelancerProfileRepository = freelancerProfileRepository;
    }
    public async Task<FreelancerProfileDto> Handle(GetFreelancerProfileByIdQuery request, CancellationToken cancellationToken)
    {
        return await _freelancerProfileRepository.GetFreelancerProfileByIdAsync(request, cancellationToken);
    }
}