namespace Freelance.Face.Domain.Interfaces;

public interface IFaceService
{
    (bool, float[]?) Process(byte[] imageBytes);
    double Distance(float[] a, float[] b);
}
