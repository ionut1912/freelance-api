using Freelance.Face.Domain.Interfaces;
using Freelance.Shared.Events.Events;
using Microsoft.Extensions.Logging;
using Shared.Rabbit.Repositories;

namespace Freelance.Face.Application.EventHandlers;

public class VerifyFaceEventHandler : IEventHandler<VerifyFaceEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<VerifyFaceEventHandler> _logger;
    private readonly IFaceService _faceService;

    public VerifyFaceEventHandler(IEventBus eventBus, ILogger<VerifyFaceEventHandler> logger, IFaceService faceService)
    {
        ArgumentNullException.ThrowIfNull(eventBus);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(faceService);
        _eventBus = eventBus;
        _logger = logger;
        _faceService = faceService;
    }

    public async Task Handle(VerifyFaceEvent @event)
    {
        byte[] img1;
        byte[] img2;

        try
        {
            img1 = Decode(@event.InitialImageUrl);
            img2 = Decode(@event.CompareImageUrl);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Invalid base64 input for ProfileId={ProfileId}", @event.ProfileId);
            await _eventBus.PublishAsync(new VerifiedFaceEvent
            {
                IsMatch = false,
                ProfileId = @event.ProfileId,
                Role = @event.Role
            });
            return;
        }

        var (h1, d1) = _faceService.Process(img1);
        var (h2, d2) = _faceService.Process(img2);

        if (!h1 || !h2 || d1 is null || d2 is null)
        {
            await _eventBus.PublishAsync(new VerifiedFaceEvent
            {
                IsMatch = false,
                ProfileId = @event.ProfileId,
                Role = @event.Role
            });
            return;
        }

        var distance = _faceService.Distance(d1, d2);

        await _eventBus.PublishAsync(new VerifiedFaceEvent
        {
            IsMatch = distance < 0.6,
            ProfileId = @event.ProfileId,
            Role = @event.Role
        });
    }

    private static byte[] Decode(string base64)
    {
        if (base64.Contains(','))
            base64 = base64[(base64.IndexOf(',') + 1)..];

        return Convert.FromBase64String(base64);
    }
}
