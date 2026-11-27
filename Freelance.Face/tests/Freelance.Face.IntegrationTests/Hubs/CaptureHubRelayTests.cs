using System.Text.Json;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace Freelance.Face.IntegrationTests.Hubs;

/// <summary>The phone-to-desktop hand-over through real SignalR connections.</summary>
public sealed class CaptureHubRelayTests(FaceApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    private async Task<(HubConnection Connection, Task<string> Photo)> ConnectAsync(string sessionId)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl(new Uri(factory.Server.BaseAddress, "/hubs/capture"), options =>
            {
                options.Transports = HttpTransportType.LongPolling;
                options.HttpMessageHandlerFactory = _ => factory.Server.CreateHandler();
            })
            .Build();
        var photo = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        connection.On<JsonElement>("PhotoReceived", message => photo.TrySetResult(message.GetProperty("dataUrl").GetString()!));
        await connection.StartAsync(_ct);
        await connection.InvokeAsync("Join", sessionId, _ct);
        return (connection, photo.Task);
    }

    [Fact]
    public async Task SendPhotoDataUrl_ReachesOnlyTheSameSession()
    {
        var session = Guid.NewGuid().ToString("N");
        var (desktop, desktopPhoto) = await ConnectAsync(session);
        var (bystander, bystanderPhoto) = await ConnectAsync(Guid.NewGuid().ToString("N"));
        var (phone, _) = await ConnectAsync(session);
        await using var d = desktop;
        await using var b = bystander;
        await using var p = phone;
        var dataUrl = "data:image/png;base64," + new string('A', 1_000_000);

        await phone.InvokeAsync("SendPhotoDataUrl", session, dataUrl, _ct);

        (await desktopPhoto.WaitAsync(TimeSpan.FromSeconds(30), _ct)).Should().Be(dataUrl);
        bystanderPhoto.IsCompleted.Should().BeFalse();
    }
}
