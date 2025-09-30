namespace Freelance.Contracts.Responses;

/// <summary>
///     Represents the result of a face comparison operation.
/// </summary>
public class FaceComparisonResult
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FaceComparisonResult" /> class.
    /// </summary>
    /// <param name="isMatch">If set to <c>true</c>, the faces match.</param>
    /// <param name="confidence">The confidence score of the match.</param>
    public FaceComparisonResult(bool isMatch, double confidence)
    {
        IsMatch = isMatch;
        Confidence = confidence;
    }

    /// <summary>
    ///     Gets a value indicating whether the faces match.
    /// </summary>
    public bool IsMatch { get; }

    /// <summary>
    ///     Gets the confidence score (inlier ratio) for the match.
    /// </summary>
    public double Confidence { get; }
}