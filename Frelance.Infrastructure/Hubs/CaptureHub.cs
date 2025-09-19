using Microsoft.AspNetCore.SignalR;

namespace Frelance.Infrastructure.Hubs;

public class CaptureHub:Hub

{
    public Task Join(string sessionId) => Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
    
    public Task SendPhotoDataUrl(string sessionId, string dataUrl)
        => Clients.Group(sessionId).SendAsync("PhotoReceived", new { dataUrl });
}