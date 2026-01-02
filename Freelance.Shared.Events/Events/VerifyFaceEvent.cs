using Shared.Rabbit.Repositories;

namespace Freelance.Shared.Events.Events;

/// <summary>
/// Represents an event that requests a face verification operation
/// between two images.
/// </summary>
/// <remarks>
/// This event is typically published to initiate an asynchronous
/// face comparison workflow.
/// </remarks>
public class VerifyFaceEvent : IEvent
{
    /// <summary>
    /// Gets a unique identifier for this event instance.
    /// </summary>
    /// <remarks>
    /// A new <see cref="Guid"/> is generated each time this property is accessed.
    /// </remarks>
    Guid IEvent.EventId => Guid.NewGuid();

    /// <summary>
    /// Gets the UTC timestamp indicating when the event occurred.
    /// </summary>
    /// <remarks>
    /// The value is generated at access time using <see cref="DateTime.UtcNow"/>.
    /// </remarks>
    DateTime IEvent.OccurredAt => DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the unique identifier of the profile
    /// associated with this face verification request.
    /// </summary>
    public Guid ProfileId { get; set; }

    /// <summary>
    /// Gets or sets the URL of the initial (reference) image
    /// used for face verification.
    /// </summary>
    public string InitialImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL of the image to be compared
    /// against the initial image.
    /// </summary>
    public string CompareImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the role or verification context
    /// associated with the face verification request.
    /// </summary>
    /// <remarks>
    /// This may represent a user role, system role,
    /// or a specific verification scenario.
    /// </remarks>
    public string Role { get; set; } = string.Empty;
}
