using JetBrains.Annotations;

namespace Frelance.Contracts.Responses;

[UsedImplicitly]
public record VerifyFaceResult(bool IsMatch, double Similarity);