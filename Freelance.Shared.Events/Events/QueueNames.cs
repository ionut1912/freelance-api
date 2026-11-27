namespace Freelance.Shared.Events.Events;

/// <summary>
/// RabbitMQ queue names used to route the integration events between services.
/// Keeping them next to the contracts guarantees publisher and consumer agree.
/// </summary>
public static class QueueNames
{
    /// <summary>Queue consumed by the Face service (<see cref="VerifyFaceEvent"/>).</summary>
    public const string FaceVerificationRequests = "face.verification-requests";

    /// <summary>Queue consumed by the UserProfiles service (<see cref="VerifiedFaceEvent"/>).</summary>
    public const string FaceVerificationResults = "profiles.face-verification-results";
}
