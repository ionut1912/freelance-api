using JetBrains.Annotations;

namespace Frelance.Contracts.Requests.Reviews;

[UsedImplicitly]
public class UpdateReviewRequest(string reviewText)
{
    public string? ReviewText { get; } = reviewText;
}