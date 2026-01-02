namespace Freelance.FaceRecognition.Domain.Models;

public class FaceFeatures
{
    public float[] Features { get; set; }=Array.Empty<float>();
    public float ColorVariance { get; set; }
    public float EdgeStrength { get; set; }
    public float CenterBrightness { get; set; }
}
