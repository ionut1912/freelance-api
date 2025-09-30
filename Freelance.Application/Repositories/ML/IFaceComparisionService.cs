using Freelance.Contracts.Responses;

namespace Freelance.Application.Repositories.ML;

public interface IFaceComparisionService
{
    Task<FaceComparisonResult> CompareFacesAsync(string base64Face1, string base64Face2);
}