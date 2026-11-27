using Freelance.Face.Domain.Interfaces;
using Freelance.Shared.Events.Events;
using Microsoft.Extensions.Logging;

namespace Freelance.Face.Application.EventHandlers;

/// <summary>
/// Handles <see cref="VerifyFaceEvent"/> messages (RabbitMQ, via Wolverine). The returned
/// <see cref="VerifiedFaceEvent"/> is a Wolverine cascading message routed back to the UserProfiles service.
/// </summary>
public sealed class FaceVerificationRequestHandler(IFaceService faceService, ILogger<FaceVerificationRequestHandler> logger)
{
    /// <summary>dlib's recommended threshold for the ResNet face-recognition model.</summary>
    public const double MatchThreshold = 0.6;

    public VerifiedFaceEvent Handle(VerifyFaceEvent request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new VerifiedFaceEvent
        {
            ProfileId = request.ProfileId,
            Role = request.Role,
            IsMatch = IsSameFace(request)
        };
    }

    private bool IsSameFace(VerifyFaceEvent request)
    {
        if (!TryDecode(request.InitialImageUrl, out var initial) || !TryDecode(request.CompareImageUrl, out var compare))
        {
            logger.LogWarning("Invalid base64 image for profile {ProfileId}", request.ProfileId);
            return false;
        }

        if (!faceService.TryGetFaceDescriptor(initial, out var d1) || !faceService.TryGetFaceDescriptor(compare, out var d2))
        {
            logger.LogInformation("Profile {ProfileId}: exactly one face is required in each image", request.ProfileId);
            return false;
        }

        var distance = faceService.Distance(d1, d2);
        var isMatch = distance < MatchThreshold;
        logger.LogInformation("Profile {ProfileId}: face distance {Distance:F4}, match = {IsMatch}", request.ProfileId,
            distance, isMatch);
        return isMatch;
    }

    /// <summary>Decodes a base64 string, optionally prefixed by a data-URL header (<c>data:image/png;base64,</c>).</summary>
    private static bool TryDecode(string base64, out byte[] bytes)
    {
        var payload = base64.AsSpan();
        var comma = payload.IndexOf(',');
        if (comma >= 0) payload = payload[(comma + 1)..];

        var buffer = new byte[payload.Length * 3 / 4 + 3];
        if (Convert.TryFromBase64Chars(payload, buffer, out var written))
        {
            bytes = buffer[..written];
            return true;
        }

        bytes = [];
        return false;
    }
}
