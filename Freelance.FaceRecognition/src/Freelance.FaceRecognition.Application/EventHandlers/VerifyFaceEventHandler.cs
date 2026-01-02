using Freelance.FaceRecognition.Domain.Exceptions;
using Freelance.FaceRecognition.Domain.Interfaces;
using Freelance.Shared.Events.Events;
using Microsoft.Extensions.Logging;
using Shared.Rabbit.Repositories;

namespace Freelance.FaceRecognition.Application.EventHandlers;

public class VerifyFaceEventHandler : IEventHandler<VerifyFaceEvent>
{
    private readonly IEventBus _eventBus;
    private ILogger<VerifyFaceEventHandler> _logger;
    private readonly IFaceComparisonRepository _faceComparisonRepository;

    public VerifyFaceEventHandler(IEventBus eventBus,ILogger<VerifyFaceEventHandler> logger, IFaceComparisonRepository faceComparisonRepository)
    {
        ArgumentNullException.ThrowIfNull(eventBus, nameof(eventBus));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(faceComparisonRepository, nameof(faceComparisonRepository));
        _eventBus = eventBus;
        _logger = logger;  
        _faceComparisonRepository = faceComparisonRepository;
    }

    public async Task Handle(VerifyFaceEvent @event)
    {
        using var image1Stream = CreateImageStream(@event.InitialImageUrl);
        using var image2Stream = CreateImageStream(@event.CompareImageUrl);

        await EnsureHumanFaceAsync(image1Stream, "initial");
        await EnsureHumanFaceAsync(image2Stream, "compare");

        image1Stream.Position = 0;
        image2Stream.Position = 0;

        var verificationResult =
            await _faceComparisonRepository.CompareFacesAsync(image1Stream, image2Stream);

        _logger.LogInformation(
            verificationResult.IsMatch
                ? "Faces match. Similarity: {Similarity}, Threshold: {Threshold}"
                : "Faces do not match. Similarity: {Similarity}, Threshold: {Threshold}",
            verificationResult.Similarity,
            verificationResult.Threshold);

        await _eventBus.PublishAsync(new VerifiedFaceEvent
        {
            IsMatch = verificationResult.IsMatch,
            ProfileId=@event.ProfileId,
            Message = verificationResult.Message,
            Role = @event.Role
        });
    }

    private static MemoryStream CreateImageStream(string base64Image)
    {
        var base64Data = base64Image[(base64Image.IndexOf(',') + 1)..];
        return new MemoryStream(Convert.FromBase64String(base64Data));
    }

    private async Task EnsureHumanFaceAsync(Stream imageStream, string imageType)
    {
        imageStream.Position = 0;
        var result = await _faceComparisonRepository.DetectHumanFaceAsync(imageStream);

        if (result.ContainsHumanFace)
            return;

        _logger.LogError(
            "The {ImageType} image does not contain a human face. Confidence: {Confidence}, Message: {Message}",
            imageType,
            result.Confidence,
            result.Message);

        throw new FaceNotHumanException(
            $"The {imageType} image does not contain a human face. " +
            $"Confidence: {result.Confidence}, Message: {result.Message}");
    }
}
