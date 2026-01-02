using System;
using System.IO;
using DlibDotNet;
using DlibDotNet.Dnn;
using Freelance.Face.Domain.Interfaces;

namespace Freelance.Face.Infrastructure.Services;

public class FaceService : IFaceService
{
    private readonly FrontalFaceDetector _detector;
    private readonly ShapePredictor _predictor;
    private readonly LossMetric _recognition;

    public FaceService()
    {
        _detector = Dlib.GetFrontalFaceDetector();
        _predictor = ShapePredictor.Deserialize(ResolveModelPath("shape_predictor_5_face_landmarks.dat"));
        _recognition = LossMetric.Deserialize(ResolveModelPath("dlib_face_recognition_resnet_model_v1.dat"));
    }

    public (bool, float[]?) Process(byte[] imageBytes)
    {
        var ext = DetectExtension(imageBytes);
        var tmp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ext);
        File.WriteAllBytes(tmp, imageBytes);

        try
        {
            using var img = Dlib.LoadImage<RgbPixel>(tmp);

            var dets = _detector.Operator(img);
            if (dets.Length != 1) return (false, null);

            using var shape = _predictor.Detect(img, dets[0]);
            var detail = Dlib.GetFaceChipDetails(shape, 150, 0.25);

            using var chip = Dlib.ExtractImageChip<RgbPixel>(img, detail);
            using var chipMatrix = new Matrix<RgbPixel>(chip);

            using var labels = _recognition.Operator<RgbPixel>(chipMatrix, 1);
            using var m = labels[0];

            var v = new float[(int)m.Columns];
            for (var i = 0; i < v.Length; i++)
                v[i] = m[0, i];

            return (true, v);
        }
        finally
        {
            try { File.Delete(tmp); } catch { }
        }
    }

    public double Distance(float[] a, float[] b)
    {
        double sum = 0;
        var n = Math.Min(a.Length, b.Length);
        for (var i = 0; i < n; i++)
        {
            var d = a[i] - b[i];
            sum += d * d;
        }
        return Math.Sqrt(sum);
    }

    private static string ResolveModelPath(string fileName)
    {
        var p1 = Path.Combine(AppContext.BaseDirectory, "models", fileName);
        if (File.Exists(p1)) return p1;

        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        for (var i = 0; i < 12 && dir != null; i++, dir = dir.Parent)
        {
            var p2 = Path.Combine(dir.FullName, "Freelance.Face", "models", fileName);
            if (File.Exists(p2)) return p2;

            var p3 = Path.Combine(dir.FullName, "models", fileName);
            if (File.Exists(p3)) return p3;
        }

        throw new FileNotFoundException(p1);
    }

    private static string DetectExtension(byte[] b)
    {
        if (b.Length >= 3 && b[0] == 0xFF && b[1] == 0xD8 && b[2] == 0xFF) return ".jpg";
        if (b.Length >= 8 && b[0] == 0x89 && b[1] == 0x50 && b[2] == 0x4E && b[3] == 0x47 && b[4] == 0x0D && b[5] == 0x0A && b[6] == 0x1A && b[7] == 0x0A) return ".png";
        if (b.Length >= 2 && b[0] == 0x42 && b[1] == 0x4D) return ".bmp";
        return ".jpg";
    }
}
