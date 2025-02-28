namespace Frelance.Application.Repositories.ML;

public interface IFaceComparisionService
{
    Task<double> CompareFacesAsync(string base64Face1, string base64Face2);
}