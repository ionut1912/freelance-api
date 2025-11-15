using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class DeleteFreelancerProfileCommandHandler:IRequestHandler<DeleteFreelancerProfileCommand,Unit>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    public DeleteFreelancerProfileCommandHandler(IFreelancerProfileRepository freelancerProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        _freelancerProfileRepository = freelancerProfileRepository;
    }
    
    public async Task<Unit> Handle(DeleteFreelancerProfileCommand request, CancellationToken cancellationToken)
    {
        var freelancerProfile =
            await _freelancerProfileRepository.GetFreelancerProfileByIdAsync(request.Id, cancellationToken);
        if (freelancerProfile is null)
            throw new ProfileNotFoundException($"Profile with id {request.Id} not found");
        await _freelancerProfileRepository.DeleteFreelancerProfileAsync(freelancerProfile, cancellationToken);
        return Unit.Value;
    }
}