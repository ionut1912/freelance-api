namespace Freelance.Face.Domain.Interfaces;

public interface IFaceService
{
    /// <summary>
    /// Detects exactly one face in the encoded image (JPEG/PNG/BMP) and returns its 128-d embedding.
    /// </summary>
    /// <returns><see langword="true"/> and the embedding when exactly one face was found.</returns>
    bool TryGetFaceDescriptor(byte[] imageBytes, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out float[]? descriptor);

    /// <summary>Euclidean distance between two face embeddings (dlib: same person when &lt; 0.6).</summary>
    double Distance(float[] a, float[] b);
}
