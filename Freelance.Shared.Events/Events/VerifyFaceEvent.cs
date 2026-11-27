namespace Freelance.Shared.Events.Events;

/// <summary>
/// Requests a face verification between a profile's reference image and a freshly captured image.
/// </summary>
/// <remarks>
/// Published by the UserProfiles service and handled by the Face service.
/// Images are base64 strings, optionally prefixed with a data-URL header (<c>data:image/png;base64,</c>).
/// </remarks>
public sealed record VerifyFaceEvent
{
    /// <summary>Unique identifier of this event instance.</summary>
    public Guid EventId { get; init; } = Guid.CreateVersion7();

    /// <summary>UTC timestamp of when the event was created.</summary>
    public DateTimeOffset OccurredAt { get; init; } = DateTimeOffset.UtcNow;

    /// <summary>Identifier of the profile being verified.</summary>
    public required Guid ProfileId { get; init; }

    /// <summary>Reference (stored) image of the profile.</summary>
    public required string InitialImageUrl { get; init; }

    /// <summary>Image to compare against the reference image.</summary>
    public required string CompareImageUrl { get; init; }

    /// <summary>Identifies the reference image; the result repeats it so a stale answer can be recognised.</summary>
    public string? ImageFingerprint { get; init; }

    /// <summary>Role of the profile owner (<c>Client</c> or <c>Freelancer</c>).</summary>
    public required string Role { get; init; }
}
