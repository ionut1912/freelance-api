using System.Net.Http.Json;
using Freelance.Shared.Events.Events;
using Freelance.UserProfiles.Application.Dtos;
using Wolverine.Tracking;

namespace Freelance.UserProfiles.IntegrationTests.Messaging;

/// <summary>End-to-end processing of <see cref="VerifiedFaceEvent"/> messages coming from the Face service.</summary>
[Collection(MessagingCollection.Name)]
public sealed class FaceVerificationResultTests(UserProfilesApiFactory factory)
{
    private readonly CancellationToken _ct = TestContext.Current.CancellationToken;

    [Fact]
    public async Task MatchingFace_VerifiesTheClientProfile()
    {
        var client = factory.CreateClient("Client");
        var profile = await UserProfilesApiFactory.CreateClientProfileAsync(client, _ct);

        await factory.Host.InvokeMessageAndWaitAsync(new VerifiedFaceEvent { ProfileId = profile.Id, Role = "Client", IsMatch = true });

        (await client.GetFromJsonAsync<ClientProfileDto>("/client-profiles/current", _ct))!.IsVerified.Should().BeTrue();
    }

    [Fact]
    public async Task MatchingFace_VerifiesTheFreelancerProfile()
    {
        var freelancer = factory.CreateClient("Freelancer");
        var profile = await UserProfilesApiFactory.CreateFreelancerProfileAsync(freelancer, _ct);

        await factory.Host.InvokeMessageAndWaitAsync(new VerifiedFaceEvent { ProfileId = profile.Id, Role = "Freelancer", IsMatch = true });

        (await freelancer.GetFromJsonAsync<FreelancerProfileDto>("/freelancer-profiles/current", _ct))!.IsVerified.Should().BeTrue();
    }

    [Fact]
    public async Task NonMatchingFace_LeavesProfileUnverified()
    {
        var client = factory.CreateClient("Client");
        var profile = await UserProfilesApiFactory.CreateClientProfileAsync(client, _ct);

        await factory.Host.InvokeMessageAndWaitAsync(new VerifiedFaceEvent { ProfileId = profile.Id, Role = "Client", IsMatch = false });

        (await client.GetFromJsonAsync<ClientProfileDto>("/client-profiles/current", _ct))!.IsVerified.Should().BeFalse();
    }
}
