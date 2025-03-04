using Frelance.Contracts.Responses;

namespace Frelance.Application.Repositories.ML;

public interface IFaceComparisionService
{
    Task<FaceComparisonResult> CompareFacesAsync(string base64Face1, string base64Face2);
}