using Freelance.Face.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Freelance.Face.UnitTests.Api.Hubs;

public sealed class CaptureHubTests
{
    private readonly Mock<IGroupManager> _groups = new();
    private readonly Mock<IHubCallerClients> _clients = new();
    private readonly Mock<HubCallerContext> _context = new();
    private readonly Mock<IClientProxy> _group = new();

    private CaptureHub CreateHub()
    {
        _context.SetupGet(c => c.ConnectionId).Returns("connection-1");
        _clients.Setup(c => c.Group("session-1")).Returns(_group.Object);
        return new CaptureHub { Groups = _groups.Object, Clients = _clients.Object, Context = _context.Object };
    }

    [Fact]
    public async Task Join_AddsTheConnectionToTheSessionGroup()
    {
        using var hub = CreateHub();

        await hub.Join("session-1");

        _groups.Verify(g => g.AddToGroupAsync("connection-1", "session-1", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SendPhotoDataUrl_BroadcastsPhotoToTheSessionGroup()
    {
        using var hub = CreateHub();

        await hub.SendPhotoDataUrl("session-1", "data:image/png;base64,AAAA");

        _group.Verify(g => g.SendCoreAsync("PhotoReceived",
            It.Is<object?[]>(args => args.Length == 1 && args[0]!.ToString()!.Contains("data:image/png;base64,AAAA")),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
