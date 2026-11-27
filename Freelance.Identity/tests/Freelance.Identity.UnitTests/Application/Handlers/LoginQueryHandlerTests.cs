using Freelance.Identity.Application.Mediatr.Accounts.Handlers;
using Freelance.Identity.Application.Mediatr.Accounts.Query;
using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.Identity.UnitTests.Application.Handlers;

public sealed class LoginQueryHandlerTests : HandlerTestBase
{
    private readonly Mock<IPasswordService> _passwords = new();
    private readonly Mock<IJwtTokenService> _tokens = new();
    private LoginQueryHandler Handler =>
        new(Accounts.Object, _passwords.Object, _tokens.Object, NullLogger<LoginQueryHandler>.Instance);

    [Fact]
    public async Task Handle_UnknownUser_ThrowsAccountNotFound() =>
        await Handler.Invoking(h => h.Handle(new LoginQuery("ghost", "x"), Ct))
            .Should().ThrowAsync<AccountNotFoundException>();

    [Fact]
    public async Task Handle_WrongPassword_ThrowsPasswordNotMatch_AndIssuesNoToken()
    {
        GivenAccountByUsername("alice", TestData.Accounts.Freelancer());
        _passwords.Setup(p => p.VerifyPassword("wrong", "hashed")).Returns(false);

        await Handler.Invoking(h => h.Handle(new LoginQuery("alice", "wrong"), Ct))
            .Should().ThrowAsync<PasswordNotMatchException>();
        _tokens.Verify(t => t.GenerateToken(It.IsAny<Account>()), Times.Never);
    }

    [Fact]
    public async Task Handle_BlockedAccount_ThrowsAccountBlocked()
    {
        var account = TestData.Accounts.Freelancer();
        account.BlockAccount();
        GivenAccountByUsername("alice", account);
        _passwords.Setup(p => p.VerifyPassword("secret", "hashed")).Returns(true);

        await Handler.Invoking(h => h.Handle(new LoginQuery("alice", "secret"), Ct))
            .Should().ThrowAsync<AccountBlockedException>();
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsDtoWithToken()
    {
        var account = TestData.Accounts.Freelancer();
        GivenAccountByUsername("alice", account);
        _passwords.Setup(p => p.VerifyPassword("secret", "hashed")).Returns(true);
        _tokens.Setup(t => t.GenerateToken(account)).Returns("jwt");

        var dto = await Handler.Handle(new LoginQuery("alice", "secret"), Ct);

        dto.Token.Should().Be("jwt");
        dto.Id.Should().Be(account.Id);
    }
}
