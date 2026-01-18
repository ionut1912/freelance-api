using Freelance.Shared.Events.Events;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Application.Mediator;
using Shared.Rabbit.Repositories;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class VerifyClientProfileCommandHandler : IRequestHandler<VerifyClientProfileCommand, Unit>
{
    private readonly IEventBus _eventBus;
    private readonly IClientProfileRepository _clientProfileRepository;
    private readonly ILogger<VerifyClientProfileCommandHandler> _logger;

    public VerifyClientProfileCommandHandler(IEventBus eventBus,IClientProfileRepository clientProfileRepository,ILogger<VerifyClientProfileCommandHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(eventBus, nameof(eventBus));
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        _eventBus = eventBus;
        _clientProfileRepository = clientProfileRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(VerifyClientProfileCommand request, CancellationToken cancellationToken = default)
    {
           var profile = await _clientProfileRepository.GetLoggedInClientProfileAsync(request.AccountId, cancellationToken);
            if (profile is null)
            {
                _logger.LogError("Client profile for accountId {AccountId} not found", request.AccountId);
            throw new ProfileNotFoundException("Client profile not found.");
            }
            await _eventBus.PublishAsync(new VerifyFaceEvent
            {
                InitialImageUrl = profile.Image,
                ProfileId = profile.Id,
                CompareImageUrl = request.ImageUrl,
                Role = "Client",
            });
      
        

        return Unit.Value;
    }
}
