using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class UpdateFreelancerRatingCommandHandler:IRequestHandler<UpdateFreelancerRatingCommand,Unit>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    
    public UpdateFreelancerRatingCommandHandler(IFreelancerProfileRepository freelancerProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository,nameof(freelancerProfileRepository));
        _freelancerProfileRepository = freelancerProfileRepository;
    }
    
    public async Task<Unit> Handle(UpdateFreelancerRatingCommand request, CancellationToken cancellationToken)
    {
        var freelancerProfile =
            await _freelancerProfileRepository.GetFreelancerProfileByIdAsync(request.Id, cancellationToken);
        if (freelancerProfile is null)
            throw new ProfileNotFoundException($"Profile with id {request.Id} not found");
        freelancerProfile.UpdateRating(request.Rating);
        await _freelancerProfileRepository.UpdateFreelancerProfileAsync(freelancerProfile, cancellationToken);
        return Unit.Value;
    }
}