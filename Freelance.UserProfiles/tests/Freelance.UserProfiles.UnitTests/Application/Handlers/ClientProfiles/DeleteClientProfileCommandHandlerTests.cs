using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.ClientProfiles;

public sealed class DeleteClientProfileCommandHandlerTests : HandlerTestBase
{
    private DeleteClientProfileCommandHandler Handler =>
        new(Clients.Object, UnitOfWork.Object, NullLogger<DeleteClientProfileCommandHandler>.Instance);

    [Fact]
    public async Task Handle_Missing_ThrowsProfileNotFound()
    {
        GivenClientById(null);

        await Handler.Invoking(h => h.Handle(new DeleteClientProfileCommand(Guid.NewGuid(), Guid.NewGuid()), Ct)).Should().ThrowAsync<ProfileNotFoundException>();
    }

    [Fact]
    public async Task Handle_Existing_DeletesAndSaves()
    {
        var profile = Profiles.Client();
        GivenClientById(profile);

        await Handler.Handle(new DeleteClientProfileCommand(profile.Id, profile.AccountId), Ct);

        Clients.Verify(r => r.Delete(profile), Times.Once);
        VerifySaved();
    }
    [Fact]
    public async Task Handle_ProfileOfAnotherAccount_ThrowsProfileNotFoundWithoutSaving()
    {
        var profile = Profiles.Client();
        GivenClientById(profile);

        await Handler.Invoking(h => h.Handle(new DeleteClientProfileCommand(profile.Id, Guid.NewGuid()), Ct))
            .Should().ThrowAsync<ProfileNotFoundException>();
        VerifyNotSaved();
    }
}
