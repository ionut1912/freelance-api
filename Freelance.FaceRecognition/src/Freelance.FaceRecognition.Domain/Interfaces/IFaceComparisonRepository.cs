using Freelance.FaceRecognition.Domain.Models;

namespace Freelance.FaceRecognition.Domain.Interfaces;

public interface IFaceComparisonRepository
{
    Task<ComparisonResult> CompareFacesAsync(Stream image1, Stream image2);
    Task<DetectionResult> DetectHumanFaceAsync(Stream image);
}
