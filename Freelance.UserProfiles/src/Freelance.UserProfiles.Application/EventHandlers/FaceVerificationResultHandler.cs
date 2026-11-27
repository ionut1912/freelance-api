using Freelance.UserProfiles.Application.Mappings;
using Freelance.Shared.Events.Events;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interfaces;

namespace Freelance.UserProfiles.Application.EventHandlers;

/// <summary>Consumes face-verification results published by the Face service (RabbitMQ, via Wolverine).</summary>
public sealed class FaceVerificationResultHandler(
    IFreelancerProfileRepository freelancerProfileRepository,
    IClientProfileRepository clientProfileRepository,
    IUnitOfWork unitOfWork,
    ILogger<FaceVerificationResultHandler> logger)
{
    public async Task Handle(VerifiedFaceEvent @event, CancellationToken cancellationToken)
    {
        // A non-matching face is an expected business outcome, not a failure: throwing here would only make
        // Wolverine retry the message and finally dead-letter it.
        if (!@event.IsMatch)
        {
            logger.LogWarning("Face verification failed for profile {ProfileId}: faces do not match", @event.ProfileId);
            return;
        }

        BaseUserProfile? profile = @event.Role switch
        {
            ProfileRoles.Client => await clientProfileRepository.GetByIdAsync(@event.ProfileId, cancellationToken),
            ProfileRoles.Freelancer => await freelancerProfileRepository.GetByIdAsync(@event.ProfileId, cancellationToken),
            _ => null
        };

        if (profile is null)
        {
            logger.LogError("Cannot verify {Role} profile {ProfileId}: profile not found", @event.Role, @event.ProfileId);
            return;
        }

        // The image may have changed while the Face service was working: that result says nothing about the new one.
        if (@event.ImageFingerprint != ImageFingerprint.Of(profile.Image))
        {
            logger.LogWarning("Ignoring a stale face verification result for {Role} profile {ProfileId}", @event.Role,
                @event.ProfileId);
            return;
        }

        profile.Verify();
        switch (profile)
        {
            case ClientProfile client: clientProfileRepository.Update(client); break;
            case FreelancerProfile freelancer: freelancerProfileRepository.Update(freelancer); break;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("{Role} profile {ProfileId} was verified", @event.Role, @event.ProfileId);
    }
}
