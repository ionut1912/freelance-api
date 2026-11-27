using Freelance.Identity.Domain.Entities;
using Freelance.Identity.Domain.Exceptions;
using Freelance.Identity.Domain.interfaces;
using Freelance.Identity.Domain.ValueObjects;
using Freelance.Identity.UnitTests.TestData;

namespace Freelance.Identity.UnitTests.Domain.Entities;

public sealed class AccountTests
{
    [Theory]
    [InlineData("", "secret", "0700000000", "alice")]
    [InlineData("a@b.io", "", "0700000000", "alice")]
    [InlineData("a@b.io", "secret", "", "alice")]
    [InlineData("a@b.io", "secret", "0700000000", " ")]
    public void Create_WithMissingValue_Throws(string email, string password, string phone, string username) =>
        FluentActions.Invoking(() => Account.Create(email, password, phone, username, Role.Client))
            .Should().Throw<ArgumentException>();

    [Fact]
    public void Create_WithNullRole_Throws() =>
        FluentActions.Invoking(() => Account.Create("a@b.io", "s", "0700000000", "alice", null!))
            .Should().Throw<ArgumentNullException>();

    [Fact]
    public void Create_SetsPropertiesAndStartsUnblocked()
    {
        var account = Account.Create("a@b.io", "secret", "0700000000", "alice", Role.Client);

        account.Should().BeEquivalentTo(new
        {
            Email = "a@b.io",
            Password = "secret",
            PhoneNumber = "0700000000",
            Username = "alice",
            Role = Role.Client,
            IsBlocked = false
        });
        account.Id.Should().NotBeEmpty();
        account.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public void BlockAccount_SetsBlockedAndTimestamps()
    {
        var account = Accounts.Freelancer();

        account.BlockAccount();

        account.IsBlocked.Should().BeTrue();
        account.BlockedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        account.UpdatedAt.Should().Be(account.BlockedAt);
    }

    [Fact]
    public void BlockAccount_WhenAlreadyBlocked_Throws()
    {
        var account = Accounts.Freelancer();
        account.BlockAccount();

        account.Invoking(a => a.BlockAccount()).Should().Throw<AccountAlreadyBlockedException>();
    }

    [Fact]
    public void UnblockAccount_WhenNotBlocked_Throws() =>
        Accounts.Freelancer().Invoking(a => a.UnblockAccount()).Should().Throw<AccountNotBlockedException>();

    [Fact]
    public void UnblockAccount_WhenBlocked_Unblocks()
    {
        var account = Accounts.Freelancer();
        account.BlockAccount();

        account.UnblockAccount();

        account.IsBlocked.Should().BeFalse();
    }

    [Fact]
    public void HashPassword_ReplacesPlainPasswordWithHash()
    {
        var passwordService = new Mock<IPasswordService>();
        passwordService.Setup(p => p.HashPassword("secret")).Returns("hashed");
        var account = Account.Create("a@b.io", "secret", "0700000000", "alice", Role.Client);

        account.HashPassword(passwordService.Object);

        account.Password.Should().Be("hashed");
    }

    [Fact]
    public void HashPassword_WithNullService_Throws() =>
        Accounts.Freelancer().Invoking(a => a.HashPassword(null!)).Should().Throw<ArgumentNullException>();
}
