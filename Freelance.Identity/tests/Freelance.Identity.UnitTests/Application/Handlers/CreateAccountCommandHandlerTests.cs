using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Mediatr.Accounts.Handlers;
using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Domain.ValueObjects;
using Freelance.Identity.UnitTests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace Freelance.Identity.UnitTests.Application.Handlers;

public sealed class CreateAccountCommandHandlerTests : HandlerTestBase
{
    private readonly Mock<IPasswordService> _passwords = new();
    private CreateAccountCommandHandler Handler =>
        new(Accounts.Object, _passwords.Object, UnitOfWork.Object, NullLogger<CreateAccountCommandHandler>.Instance);

    [Fact]
    public async Task Handle_WhenUsernameTaken_ThrowsAndDoesNotPersist()
    {
        GivenAccountByUsername("alice", TestData.Accounts.Freelancer());

        await Handler.Invoking(h => h.Handle(new CreateAccountCommand("a@b.io", "secret", "0700000000", "alice", "Client"), Ct))
            .Should().ThrowAsync<UserAlreadyExistsException>();

        Accounts.Verify(r => r.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()), Times.Never);
        UnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData("Client")]
    [InlineData("Freelancer")]
    public async Task Handle_NewAccount_HashesPasswordPersistsAndReturnsDto(string role)
    {
        _passwords.Setup(p => p.HashPassword("secret")).Returns("hashed!");

        var dto = await Handler.Handle(new CreateAccountCommand("a@b.io", "secret", "0700000000", "bob", role), Ct);

        Accounts.Verify(r => r.AddAsync(It.Is<Account>(a => a.Password == "hashed!" && a.Role == Role.FromName(role)), Ct), Times.Once);
        UnitOfWork.Verify(u => u.SaveChangesAsync(Ct), Times.Once);
        dto.Username.Should().Be("bob");
        dto.Role.Should().Be(role);
        dto.Token.Should().BeNull();
    }
}
