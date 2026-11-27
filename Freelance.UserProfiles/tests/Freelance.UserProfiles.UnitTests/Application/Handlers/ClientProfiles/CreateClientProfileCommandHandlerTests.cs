using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Handlers;
using Freelance.UserProfiles.Domain.Entities;
using Freelance.UserProfiles.Domain.Exceptions;
using Freelance.UserProfiles.UnitTests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.UserProfiles.UnitTests.Application.Handlers.ClientProfiles;

public sealed class CreateClientProfileCommandHandlerTests : HandlerTestBase
{
    private CreateClientProfileCommandHandler Handler =>
        new(Clients.Object, UnitOfWork.Object, NullLogger<CreateClientProfileCommandHandler>.Instance);

    [Fact]
    public async Task Handle_ProfileAlreadyExists_Throws()
    {
        GivenClientByAccount(Profiles.Client());

        await Handler.Invoking(h => h.Handle(new CreateClientProfileCommand(Guid.NewGuid(), Profiles.Address, "b", "i"), Ct))
            .Should().ThrowAsync<ProfileAllreadyExistsException>();
        VerifyNotSaved();
    }

    [Fact]
    public async Task Handle_NewProfile_PersistsAndReturnsDto()
    {
        var accountId = Guid.NewGuid();

        var dto = await Handler.Handle(new CreateClientProfileCommand(accountId, Profiles.Address, "b", "i"), Ct);

        Clients.Verify(r => r.AddAsync(It.Is<ClientProfile>(p => p.AccountId == accountId && p.Address.State == "B"), Ct), Times.Once);
        VerifySaved();
        dto.Should().BeEquivalentTo(new { AccountId = accountId, Address = Profiles.Address });
    }
}
