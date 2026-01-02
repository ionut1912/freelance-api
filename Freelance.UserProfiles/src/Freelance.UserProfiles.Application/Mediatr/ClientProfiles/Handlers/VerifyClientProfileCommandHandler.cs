using Freelance.Shared.Events.Events;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Shared.Application.Mediator;
using Shared.Rabbit.Repositories;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public class VerifyClientProfileCommandHandler : IRequestHandler<VerifyClientProfileCommand, Unit>
{
    private readonly IEventBus _eventBus;
    private readonly IClientProfileRepository _clientProfileRepository;
    public VerifyClientProfileCommandHandler(IEventBus eventBus,IClientProfileRepository clientProfileRepository)
    {
        ArgumentNullException.ThrowIfNull(eventBus, nameof(eventBus));
        ArgumentNullException.ThrowIfNull(clientProfileRepository, nameof(clientProfileRepository));
        _eventBus = eventBus;
        _clientProfileRepository = clientProfileRepository;
    }

    public async Task<Unit> Handle(VerifyClientProfileCommand request, CancellationToken cancellationToken = default)
    {
        var profile = await _clientProfileRepository.GetLoggedInClientProfileAsync(request.AccountId, cancellationToken);
        if (profile is null)
        {
            throw new ProfileNotFoundException("Client profile not found.");
        }

        await _eventBus.PublishAsync(new VerifyFaceEvent
        {
            InitialImageUrl= profile.Image,
            ProfileId=profile.Id,
            CompareImageUrl = request.ImageUrl,
            Role="Client",
        });

        return Unit.Value;
    }
}
