using Freelance.Face.Application.EventHandlers;
using Freelance.Face.Infrastructure.Services;
using Freelance.Shared.Events.Events;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.Face.UnitTests.Infrastructure;

/// <summary>
/// Real detection and recognition on the public-domain photos in TestData/Faces. Shares a collection with
/// <see cref="FaceServiceTests"/>, which counts temporary files and must not run alongside these.
/// </summary>
[Collection("FaceService")]
public sealed class FaceServiceRecognitionTests(FaceService service) : IClassFixture<FaceService>
{
    private static byte[] Photo(string name) =>
        File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "TestData", "Faces", name));

    private float[] Describe(string name)
    {
        service.TryGetFaceDescriptor(Photo(name), out var descriptor).Should().BeTrue($"{name} shows exactly one face");
        return descriptor!;
    }

    [Theory]
    [InlineData("person-a-1.jpg")]
    [InlineData("person-a-2.jpg")]
    [InlineData("person-b.jpg")]
    public void TryGetFaceDescriptor_SingleFace_ReturnsA128ValueEmbedding(string name) =>
        Describe(name).Should().HaveCount(128).And.OnlyContain(v => float.IsFinite(v)).And.Contain(v => v != 0);

    [Fact]
    public void TryGetFaceDescriptor_SeveralFaces_ReturnsFalse()
    {
        service.TryGetFaceDescriptor(Photo("crew.jpg"), out var descriptor).Should().BeFalse();
        descriptor.Should().BeNull();
    }

    [Fact]
    public void Distance_SamePhoto_IsZero() =>
        service.Distance(Describe("person-a-1.jpg"), Describe("person-a-1.jpg")).Should().BeApproximately(0, 1e-3);

    [Fact]
    public void Distance_SamePersonInDifferentPhotos_IsBelowTheMatchThreshold() =>
        service.Distance(Describe("person-a-1.jpg"), Describe("person-a-2.jpg"))
            .Should().BeLessThan(FaceVerificationRequestHandler.MatchThreshold);

    [Theory]
    [InlineData("person-a-1.jpg")]
    [InlineData("person-a-2.jpg")]
    public void Distance_DifferentPeople_IsNotBelowTheMatchThreshold(string name) =>
        service.Distance(Describe(name), Describe("person-b.jpg"))
            .Should().BeGreaterThanOrEqualTo(FaceVerificationRequestHandler.MatchThreshold);

    [Theory]
    [InlineData("person-a-2.jpg", true)]
    [InlineData("person-b.jpg", false)]
    [InlineData("crew.jpg", false)]
    public void Handle_WithTheRealFaceService_MatchesOnlyTheSamePerson(string compare, bool expected)
    {
        var handler = new FaceVerificationRequestHandler(service, NullLogger<FaceVerificationRequestHandler>.Instance);
        var request = new VerifyFaceEvent
        {
            ProfileId = Guid.NewGuid(),
            Role = "Client",
            InitialImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(Photo("person-a-1.jpg")),
            CompareImageUrl = Convert.ToBase64String(Photo(compare))
        };

        handler.Handle(request).IsMatch.Should().Be(expected);
    }

    /// <summary>The service is a singleton and dlib is not thread-safe.</summary>
    [Fact]
    public async Task TryGetFaceDescriptor_ConcurrentCalls_GiveTheSameResult()
    {
        var expected = Describe("person-b.jpg");

        var results = await Task.WhenAll(Enumerable.Range(0, 8)
            .Select(_ => Task.Run(() => Describe("person-b.jpg"), TestContext.Current.CancellationToken)));

        results.Should().AllSatisfy(actual => service.Distance(expected, actual).Should().BeApproximately(0, 1e-3));
    }
}
