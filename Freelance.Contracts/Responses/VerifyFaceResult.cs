using JetBrains.Annotations;

namespace Freelance.Contracts.Responses;

[UsedImplicitly]
public record VerifyFaceResult(bool IsMatch, double Similarity);