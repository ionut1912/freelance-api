using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Mediatr.Accounts.Handlers;
using Freelance.Identity.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.Identity.UnitTests.Application.Handlers;

public sealed class DeleteAccountCommandHandlerTests : HandlerTestBase
{
    private DeleteAccountCommandHandler Handler =>
        new(Accounts.Object, UnitOfWork.Object, NullLogger<DeleteAccountCommandHandler>.Instance);

    [Fact]
    public async Task Handle_MissingAccount_ThrowsAccountNotFound()
    {
        GivenAccountById(null);

        await Handler.Invoking(h => h.Handle(new DeleteAccountCommand(Guid.NewGuid()), Ct))
            .Should().ThrowAsync<AccountNotFoundException>();
    }

    [Fact]
    public async Task Handle_ExistingAccount_DeletesAndSaves()
    {
        var account = TestData.Accounts.Client();
        GivenAccountById(account);

        await Handler.Handle(new DeleteAccountCommand(account.Id), Ct);

        Accounts.Verify(r => r.Delete(account), Times.Once);
        UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }
}
