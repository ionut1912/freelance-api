using Freelance.Identity.Infrastructure.Persistance.Repositories;

namespace Freelance.Identity.UnitTests.Infrastructure;

public sealed class PasswordServiceTests
{
    private readonly PasswordService _service = new();

    [Fact]
    public void HashPassword_ProducesSaltedBcryptHash()
    {
        var first = _service.HashPassword("P@ssw0rd");
        var second = _service.HashPassword("P@ssw0rd");

        first.Should().StartWith("$2").And.NotBe("P@ssw0rd").And.NotBe(second);
    }

    [Fact]
    public void VerifyPassword_MatchesOnlyTheOriginalPassword()
    {
        var hash = _service.HashPassword("P@ssw0rd");

        _service.VerifyPassword("P@ssw0rd", hash).Should().BeTrue();
        _service.VerifyPassword("wrong", hash).Should().BeFalse();
    }
}
