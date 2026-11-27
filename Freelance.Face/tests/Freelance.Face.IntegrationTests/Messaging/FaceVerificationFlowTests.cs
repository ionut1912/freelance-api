using Freelance.Face.Domain.Interfaces;
using Freelance.Shared.Events.Events;
using Wolverine.Tracking;

namespace Freelance.Face.IntegrationTests.Messaging;

/// <summary>VerifyFaceEvent in → VerifiedFaceEvent out (cascading message routed to the UserProfiles queue).</summary>
[Collection(nameof(FaceVerificationFlowTests))]
[CollectionDefinition(nameof(FaceVerificationFlowTests), DisableParallelization = true)]
public sealed class FaceVerificationFlowTests(FaceApiFactory factory)
{
    private static VerifyFaceEvent Request() => new()
    {
        ProfileId = Guid.NewGuid(),
        Role = "Freelancer",
        InitialImageUrl = Convert.ToBase64String([1, 2, 3]),
        CompareImageUrl = Convert.ToBase64String([4, 5, 6])
    };

    private void GivenFaces(double distance)
    {
        float[]? descriptor = [0.5f];
        factory.FaceService.Setup(f => f.TryGetFaceDescriptor(It.IsAny<byte[]>(), out descriptor)).Returns(true);
        factory.FaceService.Setup(f => f.Distance(It.IsAny<float[]>(), It.IsAny<float[]>())).Returns(distance);
    }

    [Theory]
    [InlineData(0.3, true)]
    [InlineData(0.9, false)]
    public async Task VerifyFaceEvent_PublishesVerifiedFaceEvent(double distance, bool isMatch)
    {
        GivenFaces(distance);
        var request = Request();

        var session = await factory.Host.TrackActivity().Timeout(TimeSpan.FromSeconds(30)).InvokeMessageAndWaitAsync(request);

        session.Sent.SingleMessage<VerifiedFaceEvent>()
            .Should().BeEquivalentTo(new { request.ProfileId, request.Role, IsMatch = isMatch });
    }
}
