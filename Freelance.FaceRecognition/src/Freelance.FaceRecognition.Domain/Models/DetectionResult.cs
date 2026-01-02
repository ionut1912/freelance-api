namespace Freelance.FaceRecognition.Domain.Models;

public class DetectionResult
{
    public bool ContainsHumanFace { get; set; }
    public double Confidence { get; set; }
    public  string Message { get; set; }=string.Empty;
}
