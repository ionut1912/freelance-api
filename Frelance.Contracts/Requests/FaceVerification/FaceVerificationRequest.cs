using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.FaceVerification;

[UsedImplicitly]
public record FaceVerificationRequest(string FaceBase64Image);