using Freelance.Shared.Events.Events;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Shared.Application.Mediator;
using Shared.Rabbit.Repositories;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class VerifyFreelancerProfileCommandHandler : IRequestHandler<VerifyFreelancerProfileCommand, Unit>
{
    private readonly IEventBus _eventBus;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;

    public VerifyFreelancerProfileCommandHandler(IEventBus eventBus,IFreelancerProfileRepository freelancerProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(eventBus, nameof(eventBus));
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        _eventBus = eventBus;
        _freelancerProfileRepository = freelancerProfileRepository;
    }

    public async Task<Unit> Handle(VerifyFreelancerProfileCommand request, CancellationToken cancellationToken = default)
    {
        var profile = await _freelancerProfileRepository.GetLoggedInFreelancerProfileAsync(request.AccountId, cancellationToken);
        if (profile is null)
        {
            throw new ProfileNotFoundException("Freelancer profile not found.");
        }

        await _eventBus.PublishAsync(new VerifyFaceEvent
        {
            InitialImageUrl = profile.Image,
            ProfileId = profile.Id,
            CompareImageUrl = request.ImageUrl,
            Role = "Freelancer",
        });

        return Unit.Value;
    }
}
