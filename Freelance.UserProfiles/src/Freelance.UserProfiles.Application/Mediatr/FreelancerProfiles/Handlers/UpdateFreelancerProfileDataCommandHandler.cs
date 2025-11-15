using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using MediatR;

namespace Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class UpdateFreelancerProfileDataCommandHandler : IRequestHandler<UpdateFreelancerProfileDataCommand, Unit>
{
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    public UpdateFreelancerProfileDataCommandHandler(IFreelancerProfileRepository freelancerProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        _freelancerProfileRepository = freelancerProfileRepository;
    }

    public async Task<Unit> Handle(UpdateFreelancerProfileDataCommand request, CancellationToken cancellationToken)
    {
        var freelancerProfile =
            await _freelancerProfileRepository.GetFreelancerProfileByIdAsync(request.Id, cancellationToken);
        if (freelancerProfile is null)
            throw new ProfileNotFoundException($"Profile with id {request.Id} not found");

        freelancerProfile.UpdateUserData(request.Image, request.Bio);
        await _freelancerProfileRepository.UpdateFreelancerProfileAsync(freelancerProfile, cancellationToken);
        return Unit.Value;
    }
}