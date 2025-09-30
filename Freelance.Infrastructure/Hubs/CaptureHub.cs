using Microsoft.AspNetCore.SignalR;

namespace Freelance.Infrastructure.Hubs;

public class CaptureHub : Hub

{
    public Task Join(string sessionId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
    }

    public Task SendPhotoDataUrl(string sessionId, string dataUrl)
    {
        return Clients.Group(sessionId).SendAsync("PhotoReceived", new { dataUrl });
    }
}