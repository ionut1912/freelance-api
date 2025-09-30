using JetBrains.Annotations;

namespace Freelance.Contracts.Requests.Reviews;

[UsedImplicitly]
public class UpdateReviewRequest(string reviewText)
{
    public string? ReviewText { get; } = reviewText;
}