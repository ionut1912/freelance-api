using Shared.Rabbit.Repositories;

namespace Freelance.Shared.Events.Events;

/// <summary>
/// Represents an event raised when a face verification process
/// has been completed.
/// </summary>
/// <remarks>
/// This event is typically published after a face comparison operation
/// finishes, indicating the result of the verification.
/// </remarks>
public class VerifiedFaceEvent : IEvent
{
    /// <summary>
    /// Gets a unique identifier for this event instance.
    /// </summary>
    /// <remarks>
    /// A new <see cref="Guid"/> is generated each time this property is accessed.
    /// </remarks>
    public Guid EventId => Guid.NewGuid();

    /// <summary>
    /// Gets the UTC timestamp indicating when the event occurred.
    /// </summary>
    /// <remarks>
    /// The value is generated at access time using <see cref="DateTime.UtcNow"/>.
    /// </remarks>
    public DateTime OccurredAt => DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the unique identifier of the profile
    /// associated with this face verification result.
    /// </summary>
    public Guid ProfileId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the face verification
    /// was successful.
    /// </summary>
    public bool IsMatch { get; set; }

    /// <summary>
    /// Gets or sets a descriptive message providing additional
    /// information about the face verification result.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the role or verification context associated
    /// with the face verification result.
    /// </summary>
    /// <remarks>
    /// This may represent a user role, system role,
    /// or a specific verification scenario.
    /// </remarks>
    public string Role { get; set; } = string.Empty;
}
