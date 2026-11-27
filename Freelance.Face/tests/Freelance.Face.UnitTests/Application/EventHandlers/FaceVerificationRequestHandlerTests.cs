using Freelance.Face.Application.EventHandlers;
using Freelance.Face.Domain.Interfaces;
using Freelance.Shared.Events.Events;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.Face.UnitTests.Application.EventHandlers;

public sealed class FaceVerificationRequestHandlerTests
{
    private static readonly byte[] Initial = [1, 2, 3];
    private static readonly byte[] Compare = [4, 5, 6];
    private static readonly float[] D1 = [0f, 0f];
    private static readonly float[] D2 = [1f, 1f];

    private readonly Mock<IFaceService> _faces = new();

    private FaceVerificationRequestHandler Handler => new(_faces.Object, NullLogger<FaceVerificationRequestHandler>.Instance);

    private static VerifyFaceEvent Request(string initial, string compare) => new()
    {
        ProfileId = Guid.NewGuid(),
        Role = "Client",
        InitialImageUrl = initial,
        CompareImageUrl = compare
    };

    private void GivenDescriptors(float[]? first, float[]? second)
    {
        var d1 = first;
        var d2 = second;
        _faces.Setup(f => f.TryGetFaceDescriptor(It.Is<byte[]>(b => b.SequenceEqual(Initial)), out d1)).Returns(first is not null);
        _faces.Setup(f => f.TryGetFaceDescriptor(It.Is<byte[]>(b => b.SequenceEqual(Compare)), out d2)).Returns(second is not null);
    }

    [Theory]
    [InlineData(0.1, true)]
    [InlineData(0.59, true)]
    [InlineData(0.6, false)]
    [InlineData(1.2, false)]
    public void Handle_ComparesDistanceWithThreshold(double distance, bool expectedMatch)
    {
        GivenDescriptors(D1, D2);
        _faces.Setup(f => f.Distance(D1, D2)).Returns(distance);
        var request = Request(Convert.ToBase64String(Initial), Convert.ToBase64String(Compare));

        var result = Handler.Handle(request);

        result.Should().BeEquivalentTo(new { request.ProfileId, request.Role, IsMatch = expectedMatch });
    }

    [Fact]
    public void Handle_StripsDataUrlPrefixBeforeDecoding()
    {
        GivenDescriptors(D1, D2);
        _faces.Setup(f => f.Distance(D1, D2)).Returns(0.2);

        var result = Handler.Handle(Request($"data:image/png;base64,{Convert.ToBase64String(Initial)}",
            $"data:image/jpeg;base64,{Convert.ToBase64String(Compare)}"));

        result.IsMatch.Should().BeTrue();
    }

    [Theory]
    [InlineData("not base64!!", "AQID")]
    [InlineData("AQID", "%%%")]
    public void Handle_InvalidBase64_ReturnsNoMatch_WithoutCallingTheFaceService(string initial, string compare)
    {
        Handler.Handle(Request(initial, compare)).IsMatch.Should().BeFalse();

        _faces.VerifyNoOtherCalls();
    }

    [Fact]
    public void Handle_FaceNotDetected_ReturnsNoMatch()
    {
        GivenDescriptors(D1, null);

        Handler.Handle(Request(Convert.ToBase64String(Initial), Convert.ToBase64String(Compare))).IsMatch.Should().BeFalse();

        _faces.Verify(f => f.Distance(It.IsAny<float[]>(), It.IsAny<float[]>()), Times.Never);
    }

    [Fact]
    public void Handle_Null_Throws() => Handler.Invoking(h => h.Handle(null!)).Should().Throw<ArgumentNullException>();

    [Fact]
    public void MatchThreshold_IsDlibRecommendedValue() => FaceVerificationRequestHandler.MatchThreshold.Should().Be(0.6);
}
