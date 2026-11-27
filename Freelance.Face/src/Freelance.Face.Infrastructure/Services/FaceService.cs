using System.Diagnostics.CodeAnalysis;
using System.Numerics.Tensors;
using DlibDotNet;
using DlibDotNet.Dnn;
using Freelance.Face.Domain.Interfaces;

namespace Freelance.Face.Infrastructure.Services;

/// <summary>
/// dlib based face detection + ResNet face embeddings. Registered as a singleton because loading the
/// models is expensive; dlib objects are not thread-safe, so inference is serialized with a <see cref="Lock"/>.
/// </summary>
public sealed class FaceService : IFaceService, IDisposable
{
    // The input size and margin the ResNet recognition model was trained with.
    private const uint FaceChipSize = 150;
    private const double FaceChipPadding = 0.25;

    private readonly Lock _gate = new();
    private readonly FrontalFaceDetector _detector;
    private readonly ShapePredictor _predictor;
    private readonly LossMetric _recognition;

    public FaceService()
    {
        _detector = Dlib.GetFrontalFaceDetector();
        _predictor = ShapePredictor.Deserialize(DlibModelLocator.Resolve("shape_predictor_5_face_landmarks.dat"));
        _recognition = LossMetric.Deserialize(DlibModelLocator.Resolve("dlib_face_recognition_resnet_model_v1.dat"));
    }

    public bool TryGetFaceDescriptor(byte[] imageBytes, [NotNullWhen(true)] out float[]? descriptor)
    {
        using var file = new TemporaryImageFile(imageBytes);
        try
        {
            lock (_gate)
            {
                using var image = Dlib.LoadImage<RgbPixel>(file.Path);
                var detections = _detector.Operator(image);
                descriptor = detections.Length == 1 ? Describe(image, detections[0]) : null;
                return descriptor is not null;
            }
        }
        catch (ImageLoadException)
        {
            // Corrupt or unsupported image: treat as "no face" instead of failing (and retrying) the message.
            descriptor = null;
            return false;
        }
    }

    /// <summary>Aligns the detected face and runs it through the recognition network. Call while holding the lock.</summary>
    private float[] Describe(Array2D<RgbPixel> image, Rectangle face)
    {
        using var shape = _predictor.Detect(image, face);
        var chipDetails = Dlib.GetFaceChipDetails(shape, FaceChipSize, FaceChipPadding);
        using var chip = Dlib.ExtractImageChip<RgbPixel>(image, chipDetails);
        using var chipMatrix = new Matrix<RgbPixel>(chip);

        using var embeddings = _recognition.Operator(chipMatrix, 1);
        using var embedding = embeddings[0];

        // dlib returns a column vector (128 rows x 1 column). Sizing by Columns kept only the first value,
        // which made different people match each other.
        var descriptor = new float[embedding.Rows];
        for (var i = 0; i < descriptor.Length; i++)
            descriptor[i] = embedding[i, 0];

        return descriptor;
    }

    // SIMD-accelerated Euclidean distance.
    public double Distance(float[] a, float[] b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        var length = Math.Min(a.Length, b.Length);
        return TensorPrimitives.Distance(a.AsSpan(0, length), b.AsSpan(0, length));
    }

    public void Dispose()
    {
        _recognition.Dispose();
        _predictor.Dispose();
        _detector.Dispose();
    }
}
