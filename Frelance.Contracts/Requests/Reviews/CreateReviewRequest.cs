using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.Reviews;

[UsedImplicitly]
public record CreateReviewRequest(string ReviewText);