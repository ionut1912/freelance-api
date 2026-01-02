namespace Freelance.FaceRecognition.Domain.Models;

public class ComparisonResult
{
    public bool IsMatch { get; set; }
    public double Similarity { get; set; }
    public double Threshold { get; set; }
    public bool Image1ContainsHuman { get; set; }
    public bool Image2ContainsHuman { get; set; }
    public double Image1Confidence { get; set; }
    public double Image2Confidence { get; set; }
    public  string Message { get; set; }=string.Empty;
}
