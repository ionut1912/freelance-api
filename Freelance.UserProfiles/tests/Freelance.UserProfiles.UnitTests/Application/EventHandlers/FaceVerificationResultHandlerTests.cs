using Freelance.Shared.Events.Events;
using Freelance.UserProfiles.Application.EventHandlers;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.UnitTests.Application.Handlers;
using Freelance.UserProfiles.UnitTests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.UserProfiles.UnitTests.Application.EventHandlers;

public sealed class FaceVerificationResultHandlerTests : HandlerTestBase
{
    private FaceVerificationResultHandler Handler =>
        new(Freelancers.Object, Clients.Object, UnitOfWork.Object, NullLogger<FaceVerificationResultHandler>.Instance);

    private static VerifiedFaceEvent Result(Guid profileId, string role, bool isMatch, string? image = null) =>
        new() { ProfileId = profileId, Role = role, IsMatch = isMatch, ImageFingerprint = image is null ? null : ImageFingerprint.Of(image) };

    [Fact]
    public async Task Handle_NoMatch_DoesNothing_AndDoesNotThrow()
    {
        await Handler.Handle(Result(Guid.NewGuid(), "Client", isMatch: false), Ct);

        Clients.VerifyNoOtherCalls();
        VerifyNotSaved();
    }

    [Fact]
    public async Task Handle_MatchForClient_VerifiesAndSaves()
    {
        var profile = Profiles.Client();
        GivenClientById(profile);

        await Handler.Handle(Result(profile.Id, "Client", isMatch: true, profile.Image), Ct);

        profile.IsVerified.Should().BeTrue();
        Clients.Verify(r => r.Update(profile), Times.Once);
        VerifySaved();
    }

    [Fact]
    public async Task Handle_MatchForFreelancer_VerifiesAndSaves()
    {
        var profile = Profiles.Freelancer();
        GivenFreelancerById(profile);

        await Handler.Handle(Result(profile.Id, "Freelancer", isMatch: true, profile.Image), Ct);

        profile.IsVerified.Should().BeTrue();
        Freelancers.Verify(r => r.Update(profile), Times.Once);
        VerifySaved();
    }

    [Theory]
    [InlineData("an older image")]
    [InlineData(null)]
    public async Task Handle_MatchForAnotherImage_IsIgnored(string? requestedImage)
    {
        var profile = Profiles.Client();
        GivenClientById(profile);

        await Handler.Handle(Result(profile.Id, "Client", isMatch: true, requestedImage), Ct);

        profile.IsVerified.Should().BeFalse();
        VerifyNotSaved();
    }

    [Theory]
    [InlineData("Client")]
    [InlineData("Admin")]
    public async Task Handle_UnknownProfileOrRole_IsIgnored(string role)
    {
        GivenClientById(null);

        await Handler.Handle(Result(Guid.NewGuid(), role, isMatch: true), Ct);

        VerifyNotSaved();
    }
}
