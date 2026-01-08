using Freelance.Shared.Events.Events;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Rabbit.Repositories;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public class VerifyFreelancerProfileCommandHandler : IRequestHandler<VerifyFreelancerProfileCommand, Unit>
{
    private readonly IEventBus _eventBus;
    private readonly IFreelancerProfileRepository _freelancerProfileRepository;
    private readonly ILogger<VerifyFreelancerProfileCommandHandler> _logger;

    public VerifyFreelancerProfileCommandHandler(IEventBus eventBus,IFreelancerProfileRepository freelancerProfileRepository,ILogger<VerifyFreelancerProfileCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(eventBus, nameof(eventBus));
        ArgumentNullException.ThrowIfNull(freelancerProfileRepository, nameof(freelancerProfileRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _eventBus = eventBus;
        _freelancerProfileRepository = freelancerProfileRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(VerifyFreelancerProfileCommand request, CancellationToken cancellationToken = default)
    {
        var profile = await _freelancerProfileRepository.GetLoggedInFreelancerProfileAsync(request.AccountId, cancellationToken);
        if (profile is null)
        {
            _logger.LogError("Freelancer profile for accountId {AccountId} not found", request.AccountId);
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
