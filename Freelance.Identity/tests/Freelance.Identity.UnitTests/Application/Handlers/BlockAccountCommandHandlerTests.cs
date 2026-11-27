using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Mediatr.Accounts.Handlers;
using Freelance.Identity.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.Identity.UnitTests.Application.Handlers;

public sealed class BlockAccountCommandHandlerTests : HandlerTestBase
{
    private BlockAccountCommandHandler Handler =>
        new(Accounts.Object, UnitOfWork.Object, NullLogger<BlockAccountCommandHandler>.Instance);

    [Fact]
    public async Task Handle_MissingAccount_ThrowsAccountNotFound()
    {
        GivenAccountById(null);

        await Handler.Invoking(h => h.Handle(new BlockAccountCommand(Guid.NewGuid()), Ct))
            .Should().ThrowAsync<AccountNotFoundException>();
    }

    [Fact]
    public async Task Handle_ExistingAccount_BlocksAndSaves()
    {
        var account = TestData.Accounts.Freelancer();
        GivenAccountById(account);

        await Handler.Handle(new BlockAccountCommand(account.Id), Ct);

        account.IsBlocked.Should().BeTrue();
        Accounts.Verify(r => r.Update(account), Times.Once);
        UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }
}
