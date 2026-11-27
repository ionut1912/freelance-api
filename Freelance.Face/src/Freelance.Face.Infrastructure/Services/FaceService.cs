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
    private readonly Lock _gate = new();
    private readonly FrontalFaceDetector _detector;
    private readonly ShapePredictor _predictor;
    private readonly LossMetric _recognition;

    public FaceService()
    {
        _detector = Dlib.GetFrontalFaceDetector();
        _predictor = ShapePredictor.Deserialize(ResolveModelPath("shape_predictor_5_face_landmarks.dat"));
        _recognition = LossMetric.Deserialize(ResolveModelPath("dlib_face_recognition_resnet_model_v1.dat"));
    }

    public bool TryGetFaceDescriptor(byte[] imageBytes, [NotNullWhen(true)] out float[]? descriptor)
    {
        ArgumentNullException.ThrowIfNull(imageBytes);

        // DlibDotNet only decodes images from disk.
        var tmp = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}{DetectExtension(imageBytes)}");
        File.WriteAllBytes(tmp, imageBytes);

        try
        {
            lock (_gate)
            {
                using var img = Dlib.LoadImage<RgbPixel>(tmp);

                var detections = _detector.Operator(img);
                if (detections.Length != 1)
                {
                    descriptor = null;
                    return false;
                }

                using var shape = _predictor.Detect(img, detections[0]);
                var chipDetails = Dlib.GetFaceChipDetails(shape, 150, 0.25);
                using var chip = Dlib.ExtractImageChip<RgbPixel>(img, chipDetails);
                using var chipMatrix = new Matrix<RgbPixel>(chip);
                using var embeddings = _recognition.Operator(chipMatrix, 1);
                using var embedding = embeddings[0];

                descriptor = new float[embedding.Columns];
                for (var i = 0; i < descriptor.Length; i++)
                    descriptor[i] = embedding[0, i];

                return true;
            }
        }
        catch (ImageLoadException)
        {
            // Corrupt or unsupported image: treat as "no face" instead of failing (and retrying) the message.
            descriptor = null;
            return false;
        }
        finally
        {
            File.Delete(tmp);
        }
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

    private static string ResolveModelPath(string fileName)
    {
        var defaultPath = Path.Combine(AppContext.BaseDirectory, "models", fileName);
        if (File.Exists(defaultPath)) return defaultPath;

        // Development fallback: walk up from bin/ to find the repository's models folder.
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            string[] candidates = [Path.Combine(dir.FullName, "Freelance.Face", "models", fileName), Path.Combine(dir.FullName, "models", fileName)];
            if (candidates.FirstOrDefault(File.Exists) is { } found) return found;
        }

        throw new FileNotFoundException($"dlib model '{fileName}' was not found.", defaultPath);
    }

    private static string DetectExtension(ReadOnlySpan<byte> b) => b switch
    {
        [0xFF, 0xD8, 0xFF, ..] => ".jpg",
        [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, ..] => ".png",
        [0x42, 0x4D, ..] => ".bmp",
        _ => ".jpg"
    };
}
