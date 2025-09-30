using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.FaceVerification;

[UsedImplicitly]
public record FaceVerificationRequest(string FaceBase64Image);