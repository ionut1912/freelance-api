using System.Net;
using System.Net.Http.Json;
using Freelance.Shared.Events.Events;
using Wolverine;
using Wolverine.Tracking;

namespace Freelance.UserProfiles.IntegrationTests.Messaging;

/// <summary>PUT .../verify publishes a <see cref="VerifyFaceEvent"/> (through the durable outbox) for the Face service.</summary>
[Collection(MessagingCollection.Name)]
public sealed class VerifyFaceRequestTests(UserProfilesApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    private async Task<(HttpResponseMessage Response, ITrackedSession Session)> PutAndTrackAsync(HttpClient client, string url, object body)
    {
        HttpResponseMessage? response = null;
        var session = await factory.Host.TrackActivity().Timeout(TimeSpan.FromSeconds(30))
            .ExecuteAndWaitAsync((Func<IMessageContext, Task>)(async _ => response = await client.PutAsJsonAsync(url, body, _ct)));
        return (response!, session);
    }

    [Fact]
    public async Task VerifyClient_PublishesVerifyFaceEvent()
    {
        var client = factory.CreateClient("Client");
        var profile = await UserProfilesApiFactory.CreateClientProfileAsync(client, _ct);

        var (response, session) = await PutAndTrackAsync(client, "/client-profiles/verify", new { imageUrl = "captured" });

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        session.Sent.SingleMessage<VerifyFaceEvent>().Should().BeEquivalentTo(new
        {
            ProfileId = profile.Id,
            Role = "Client",
            InitialImageUrl = "data:image/png;base64,AAAA",
            CompareImageUrl = "captured"
        });
    }

    [Fact]
    public async Task VerifyFreelancer_PublishesVerifyFaceEvent()
    {
        var freelancer = factory.CreateClient("Freelancer");
        var profile = await UserProfilesApiFactory.CreateFreelancerProfileAsync(freelancer, _ct);

        var (response, session) = await PutAndTrackAsync(freelancer, "/freelancer-profiles/verify", new { imageUrl = "captured" });

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        session.Sent.SingleMessage<VerifyFaceEvent>()
            .Should().BeEquivalentTo(new { ProfileId = profile.Id, Role = "Freelancer", CompareImageUrl = "captured" });
    }
}
