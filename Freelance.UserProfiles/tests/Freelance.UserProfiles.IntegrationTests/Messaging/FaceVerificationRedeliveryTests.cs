using System.Net.Http.Json;
using Freelance.Shared.Events.Events;
using Freelance.UserProfiles.Application.Dtos;
using Wolverine.Tracking;

namespace Freelance.UserProfiles.IntegrationTests.Messaging;

[Collection(MessagingCollection.Name)]
public sealed class FaceVerificationRedeliveryTests(UserProfilesApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    /// <summary>RabbitMQ delivers at least once, so the same result may arrive again.</summary>
    [Fact]
    public async Task MatchingResult_DeliveredTwice_LeavesTheProfileVerified()
    {
        var client = factory.CreateClient("Client");
        var profile = await UserProfilesApiFactory.CreateClientProfileAsync(client, _ct);
        var result = new VerifiedFaceEvent { ProfileId = profile.Id, Role = "Client", IsMatch = true };

        await factory.Host.InvokeMessageAndWaitAsync(result);
        await factory.Host.InvokeMessageAndWaitAsync(result);

        (await client.GetFromJsonAsync<ClientProfileDto>("/client-profiles/current", _ct))!.IsVerified.Should().BeTrue();
    }
}
