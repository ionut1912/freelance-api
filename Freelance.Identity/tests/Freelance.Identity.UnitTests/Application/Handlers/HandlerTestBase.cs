using System.Linq.Expressions;
using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.interfaces;
using Shared.Domain.Interfaces;

namespace Freelance.Identity.UnitTests.Application.Handlers;

public abstract class HandlerTestBase
{
    protected Mock<IAccountRepository> Accounts { get; } = new();
    protected Mock<IUnitOfWork> UnitOfWork { get; } = new();
    protected CancellationToken Ct { get; } = TestContext.Current.CancellationToken;

    protected void GivenAccountById(Account? account) =>
        Accounts.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(),
                It.IsAny<Expression<Func<Account, object>>[]>()))
            .ReturnsAsync(account!);

    protected void GivenAccountByUsername(string username, Account? account) =>
        Accounts.Setup(r => r.GetAccountByUsernameAsync(username, It.IsAny<CancellationToken>())).ReturnsAsync(account);
}
