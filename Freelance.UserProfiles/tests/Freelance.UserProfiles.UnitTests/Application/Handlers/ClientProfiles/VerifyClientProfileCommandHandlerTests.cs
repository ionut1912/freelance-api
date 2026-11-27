using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.ClientProfiles;

public sealed class VerifyClientProfileCommandHandlerTests : HandlerTestBase
{
    private VerifyClientProfileCommandHandler Handler => new(Clients.Object, NullLogger<VerifyClientProfileCommandHandler>.Instance);

    [Fact]
    public async Task Handle_Missing_ThrowsProfileNotFound()
    {
        GivenClientByAccount(null);

        await Handler.Invoking(h => h.Handle(new VerifyClientProfileCommand(Guid.NewGuid(), "url"), Ct))
            .Should().ThrowAsync<ProfileNotFoundException>();
    }

    [Fact]
    public async Task Handle_Existing_ReturnsVerifyFaceEventForTheClient()
    {
        var profile = Profiles.Client();
        GivenClientByAccount(profile);

        var message = await Handler.Handle(new VerifyClientProfileCommand(profile.AccountId, "captured"), Ct);

        message.Should().BeEquivalentTo(new { ProfileId = profile.Id, InitialImageUrl = "img", CompareImageUrl = "captured", Role = "Client" });
    }
}
