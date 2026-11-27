namespace Freelance.Shared.Events.Events;

/// <summary>
/// Result of a face verification requested through <see cref="VerifyFaceEvent"/>.
/// </summary>
/// <remarks>
/// Published by the Face service and handled by the UserProfiles service.
/// </remarks>
public sealed record VerifiedFaceEvent
{
    /// <summary>Unique identifier of this event instance.</summary>
    public Guid EventId { get; init; } = Guid.CreateVersion7();

    /// <summary>UTC timestamp of when the event was created.</summary>
    public DateTimeOffset OccurredAt { get; init; } = DateTimeOffset.UtcNow;

    /// <summary>Identifier of the profile that was verified.</summary>
    public required Guid ProfileId { get; init; }

    /// <summary><see langword="true"/> when both images contain the same face.</summary>
    public required bool IsMatch { get; init; }

    /// <summary><see cref="VerifyFaceEvent.ImageFingerprint"/> of the request this result answers.</summary>
    public string? ImageFingerprint { get; init; }

    /// <summary>Role of the profile owner (<c>Client</c> or <c>Freelancer</c>).</summary>
    public required string Role { get; init; }
}
