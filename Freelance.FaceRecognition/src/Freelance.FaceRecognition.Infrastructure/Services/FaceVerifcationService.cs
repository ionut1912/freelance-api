using Freelance.FaceRecognition.Domain.Interfaces;
using Freelance.FaceRecognition.Domain.Models;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Freelance.FaceRecognition.Infrastructure.Services;

public class FaceVerifcationService : IFaceComparisonRepository
{
    private readonly ILogger<FaceVerifcationService> _logger;
    private const double DEFAULT_MATCH_THRESHOLD = 0.65;
    private const double HUMAN_FACE_THRESHOLD = 0.3;

    public FaceVerifcationService(ILogger<FaceVerifcationService> logger)
    {
        ArgumentNullException.ThrowIfNull(logger,nameof(logger));
        _logger = logger;
    }

    public async Task<ComparisonResult> CompareFacesAsync(
         Stream image1Stream,
         Stream image2Stream)
    {
        try
        {
            var threshold = DEFAULT_MATCH_THRESHOLD;


            var features1 = await ExtractFaceFeatures(image1Stream);
            var features2 = await ExtractFaceFeatures(image2Stream);

            var isHuman1 = IsLikelyHumanFace(features1);
            var isHuman2 = IsLikelyHumanFace(features2);

            if (!isHuman1.IsHuman || !isHuman2.IsHuman)
            {
                return new ComparisonResult
                {
                    IsMatch = false,
                    Similarity = 0,
                    Threshold = threshold,
                    Image1ContainsHuman = isHuman1.IsHuman,
                    Image2ContainsHuman = isHuman2.IsHuman,
                    Image1Confidence = isHuman1.Confidence,
                    Image2Confidence = isHuman2.Confidence,
                    Message = !isHuman1.IsHuman && !isHuman2.IsHuman
                        ? "Neither image contains a detectable human face"
                        : !isHuman1.IsHuman
                            ? "First image does not contain a detectable human face"
                            : "Second image does not contain a detectable human face"
                };
            }
            var similarity = CalculateSimilarity(features1.Features, features2.Features);
            var isMatch = similarity >= threshold;

            _logger.LogInformation($"Face comparison: Similarity={similarity:F4}, Match={isMatch}");

            return new ComparisonResult
            {
                IsMatch = isMatch,
                Similarity = Math.Round(similarity, 4),
                Threshold = threshold,
                Image1ContainsHuman = true,
                Image2ContainsHuman = true,
                Image1Confidence = isHuman1.Confidence,
                Image2Confidence = isHuman2.Confidence,
                Message = isMatch
                    ? "The faces match - likely the same person"
                    : "The faces do not match - likely different persons"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error comparing faces");
            return new ComparisonResult
            {
                IsMatch = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    public async Task<DetectionResult> DetectHumanFaceAsync(Stream imageStream)
    {
        try
        {
            var features = await ExtractFaceFeatures(imageStream);
            var detection = IsLikelyHumanFace(features);

            _logger.LogInformation($"Face detection: IsHuman={detection.IsHuman}, Confidence={detection.Confidence:F4}");

            return new DetectionResult
            {
                ContainsHumanFace = detection.IsHuman,
                Confidence = detection.Confidence,
                Message = detection.IsHuman
                    ? "Human face detected in image"
                    : "No human face detected in image"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting face");
            return new DetectionResult
            {
                ContainsHumanFace = false,
                Confidence = 0,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    private async Task<FaceFeatures> ExtractFaceFeatures(Stream imageStream)
    {
        using var image = await Image.LoadAsync<Rgb24>(imageStream);

        image.Mutate(x => x.Resize(128, 128));

        var features = new float[128];
        var colorVariance = 0f;
        var edgeStrength = 0f;
        var centerBrightness = 0f;
        var pixelIndex = 0;

        image.ProcessPixelRows(accessor =>
        {
            var prevPixel = new Rgb24(0, 0, 0);

            for (int y = 0; y < accessor.Height && pixelIndex < 128; y++)
            {
                var row = accessor.GetRowSpan(y);
                for (int x = 0; x < row.Length && pixelIndex < 128; x++)
                {
                    var pixel = row[x];
                    var normalized = (pixel.R + pixel.G + pixel.B) / 765f;
                    features[pixelIndex++] = normalized;

                    colorVariance += Math.Abs(pixel.R - pixel.G) + Math.Abs(pixel.G - pixel.B);

                    if (x > 0)
                    {
                        edgeStrength += Math.Abs(pixel.R - prevPixel.R);
                    }

                    // Center brightness (face typically in center)
                    if (x > 40 && x < 88 && y > 40 && y < 88)
                    {
                        centerBrightness += normalized;
                    }

                    prevPixel = pixel;
                }
            }
        });

        var magnitude = Math.Sqrt(features.Sum(f => f * f));
        if (magnitude > 0)
        {
            for (int i = 0; i < features.Length; i++)
            {
                features[i] /= (float)magnitude;
            }
        }

        return new FaceFeatures
        {
            Features = features,
            ColorVariance = colorVariance / (128 * 128),
            EdgeStrength = edgeStrength / (128 * 128),
            CenterBrightness = centerBrightness / (48 * 48)
        };
    }

    private (bool IsHuman, double Confidence) IsLikelyHumanFace(FaceFeatures features)
    {
        // Simple heuristics for human face detection
        // A human face typically has:
        // 1. Moderate color variance (skin tones)
        // 2. Strong edges (facial features)
        // 3. Center brightness (face usually centered and well-lit)

        var colorScore = Math.Min(features.ColorVariance / 100f, 1.0);
        var edgeScore = Math.Min(features.EdgeStrength / 50f, 1.0);
        var brightnessScore = features.CenterBrightness > 0.2 && features.CenterBrightness < 0.9 ? 1.0 : 0.3;

        var confidence = (colorScore * 0.3 + edgeScore * 0.4 + brightnessScore * 0.3);
        var isHuman = confidence >= HUMAN_FACE_THRESHOLD;

        return (isHuman, Math.Round(confidence, 4));
    }

    private double CalculateSimilarity(float[] features1, float[] features2)
    {
        // Cosine similarity
        double dotProduct = 0;
        for (int i = 0; i < features1.Length; i++)
        {
            dotProduct += features1[i] * features2[i];
        }
        return Math.Max(0, dotProduct); // Ensure non-negative
    }
}
