using Freelance.UserProfiles.Application.Mappings;
using Freelance.Shared.Events.Events;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;

public sealed class VerifyFreelancerProfileCommandHandler(
    IFreelancerProfileRepository freelancerProfileRepository,
    ILogger<VerifyFreelancerProfileCommandHandler> logger)
{
    /// <summary>
    /// Returns the <see cref="VerifyFaceEvent"/> as a Wolverine cascading message: Wolverine publishes it
    /// through the durable outbox to the Face service queue once the handler succeeds.
    /// </summary>
    public async Task<VerifyFaceEvent> Handle(VerifyFreelancerProfileCommand command, CancellationToken cancellationToken)
    {
        var profile = await freelancerProfileRepository.GetLoggedInFreelancerProfileAsync(command.AccountId, cancellationToken)
                      ?? throw new ProfileNotFoundException("Freelancer profile not found.");

        logger.LogInformation("Face verification requested for freelancer profile {ProfileId}", profile.Id);
        return new VerifyFaceEvent
        {
            ProfileId = profile.Id,
            InitialImageUrl = profile.Image,
            ImageFingerprint = ImageFingerprint.Of(profile.Image),
            CompareImageUrl = command.ImageUrl,
            Role = ProfileRoles.Freelancer
        };
    }
}
