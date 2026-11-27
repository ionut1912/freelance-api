using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.ClientProfiles;

public sealed class UpdateClientProfileAddressCommandHandlerTests : HandlerTestBase
{
    private UpdateClientProfileAddressCommandHandler Handler =>
        new(Clients.Object, UnitOfWork.Object, NullLogger<UpdateClientProfileAddressCommandHandler>.Instance);

    [Fact]
    public async Task Handle_Missing_ThrowsProfileNotFound()
    {
        GivenClientById(null);

        await Handler.Invoking(h => h.Handle(new UpdateClientProfileAddressCommand(Guid.NewGuid(), Profiles.Address, Guid.NewGuid()), Ct))
            .Should().ThrowAsync<ProfileNotFoundException>();
    }

    [Fact]
    public async Task Handle_Existing_UpdatesAddressAndSaves()
    {
        var profile = Profiles.Client();
        GivenClientById(profile);

        await Handler.Handle(new UpdateClientProfileAddressCommand(profile.Id, new AddressDto("S", "Cluj", "CJ", "4", "RO", "1"), profile.AccountId), Ct);

        profile.Address.City.Should().Be("Cluj");
        Clients.Verify(r => r.Update(profile), Times.Once);
        VerifySaved();
    }
    [Fact]
    public async Task Handle_ProfileOfAnotherAccount_ThrowsProfileNotFoundWithoutSaving()
    {
        var profile = Profiles.Client();
        GivenClientById(profile);

        await Handler.Invoking(h => h.Handle(new UpdateClientProfileAddressCommand(profile.Id, Profiles.Address, Guid.NewGuid()), Ct))
            .Should().ThrowAsync<ProfileNotFoundException>();
        VerifyNotSaved();
    }
}
