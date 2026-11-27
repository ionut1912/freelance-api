using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Mediatr.Accounts.Handlers;
using Freelance.Identity.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.Identity.UnitTests.Application.Handlers;

public sealed class UnblockAccountCommandHandlerTests : HandlerTestBase
{
    private UnblockAccountCommandHandler Handler =>
        new(Accounts.Object, UnitOfWork.Object, NullLogger<UnblockAccountCommandHandler>.Instance);

    [Fact]
    public async Task Handle_MissingAccount_ThrowsAccountNotFound()
    {
        GivenAccountById(null);

        await Handler.Invoking(h => h.Handle(new UnblockAccountCommand(Guid.NewGuid()), Ct))
            .Should().ThrowAsync<AccountNotFoundException>();
    }

    [Fact]
    public async Task Handle_AccountNotBlocked_ThrowsAccountNotBlocked()
    {
        GivenAccountById(TestData.Accounts.Freelancer());

        await Handler.Invoking(h => h.Handle(new UnblockAccountCommand(Guid.NewGuid()), Ct))
            .Should().ThrowAsync<AccountNotBlockedException>();
    }

    [Fact]
    public async Task Handle_BlockedAccount_UnblocksAndSaves()
    {
        var account = TestData.Accounts.Freelancer();
        account.BlockAccount();
        GivenAccountById(account);

        await Handler.Handle(new UnblockAccountCommand(account.Id), Ct);

        account.IsBlocked.Should().BeFalse();
        Accounts.Verify(r => r.Update(account), Times.Once);
        UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
    }
}
