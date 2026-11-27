using Freelance.Shared.Events.Events;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;

public sealed class VerifyClientProfileCommandHandler(
    IClientProfileRepository clientProfileRepository,
    ILogger<VerifyClientProfileCommandHandler> logger)
{
    /// <summary>
    /// Returns the <see cref="VerifyFaceEvent"/> as a Wolverine cascading message: Wolverine publishes it
    /// through the durable outbox to the Face service queue once the handler succeeds.
    /// </summary>
    public async Task<VerifyFaceEvent> Handle(VerifyClientProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await clientProfileRepository.GetLoggedInClientProfileAsync(command.AccountId, cancellationToken)
                      ?? throw new ProfileNotFoundException("Client profile not found.");

        logger.LogInformation("Face verification requested for client profile {ProfileId}", profile.Id);
        return new VerifyFaceEvent
        {
            ProfileId = profile.Id,
            InitialImageUrl = profile.Image,
            CompareImageUrl = command.ImageUrl,
            Role = ProfileRoles.Client
        };
    }
}
