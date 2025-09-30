using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.Reviews;

[UsedImplicitly]
public record CreateReviewRequest(string ReviewText);