using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Handlers;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.FreelancerProfiles;

public sealed class VerifyFreelancerProfileCommandHandlerTests : HandlerTestBase
{
    private VerifyFreelancerProfileCommandHandler Handler =>
        new(Freelancers.Object, NullLogger<VerifyFreelancerProfileCommandHandler>.Instance);

    [Fact]
    public async Task Handle_Missing_ThrowsProfileNotFound()
    {
        GivenFreelancerByAccount(null);

        await Handler.Invoking(h => h.Handle(new VerifyFreelancerProfileCommand(Guid.NewGuid(), "url"), Ct))
            .Should().ThrowAsync<ProfileNotFoundException>();
    }

    [Fact]
    public async Task Handle_Existing_ReturnsVerifyFaceEventForTheFreelancer()
    {
        var profile = Profiles.Freelancer();
        GivenFreelancerByAccount(profile);

        var message = await Handler.Handle(new VerifyFreelancerProfileCommand(profile.AccountId, "captured"), Ct);

        message.Should().BeEquivalentTo(new { ProfileId = profile.Id, InitialImageUrl = "img", CompareImageUrl = "captured", Role = "Freelancer" });
        message.EventId.Should().NotBeEmpty();
    }
}
