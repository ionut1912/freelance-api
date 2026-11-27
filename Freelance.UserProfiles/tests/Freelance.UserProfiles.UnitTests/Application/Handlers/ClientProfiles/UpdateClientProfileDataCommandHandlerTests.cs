using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.ClientProfiles;

public sealed class UpdateClientProfileDataCommandHandlerTests : HandlerTestBase
{
    private UpdateClientProfileDataCommandHandler Handler =>
        new(Clients.Object, UnitOfWork.Object, NullLogger<UpdateClientProfileDataCommandHandler>.Instance);

    [Fact]
    public async Task Handle_Missing_ThrowsProfileNotFound()
    {
        GivenClientById(null);

        await Handler.Invoking(h => h.Handle(new UpdateClientProfileDataCommand(Guid.NewGuid(), "b", "i", Guid.NewGuid()), Ct))
            .Should().ThrowAsync<ProfileNotFoundException>();
    }

    [Fact]
    public async Task Handle_SameImage_ThrowsWithoutSaving()
    {
        var existing = Profiles.Client();
        GivenClientById(existing);

        await Handler.Invoking(h => h.Handle(new UpdateClientProfileDataCommand(Guid.NewGuid(), "new", "img", existing.AccountId), Ct))
            .Should().ThrowAsync<ImageAlreadyExistsException>();
        VerifyNotSaved();
    }

    [Fact]
    public async Task Handle_Existing_UpdatesAndSaves()
    {
        var profile = Profiles.Client();
        GivenClientById(profile);

        await Handler.Handle(new UpdateClientProfileDataCommand(profile.Id, "New bio", "new-img", profile.AccountId), Ct);

        profile.Bio.Should().Be("New bio");
        VerifySaved();
    }
    [Fact]
    public async Task Handle_ProfileOfAnotherAccount_ThrowsProfileNotFoundWithoutSaving()
    {
        var profile = Profiles.Client();
        GivenClientById(profile);

        await Handler.Invoking(h => h.Handle(new UpdateClientProfileDataCommand(profile.Id, "New bio", "new-img", Guid.NewGuid()), Ct))
            .Should().ThrowAsync<ProfileNotFoundException>();
        VerifyNotSaved();
    }
}
