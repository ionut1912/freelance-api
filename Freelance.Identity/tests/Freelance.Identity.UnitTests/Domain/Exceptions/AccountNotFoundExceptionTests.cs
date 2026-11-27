using Freelance.Identity.Domain.Exceptions;

namespace Freelance.Identity.UnitTests.Domain.Exceptions;

public sealed class AccountNotFoundExceptionTests
{
    [Fact]
    public void Constructor_KeepsMessage_AndIsAnException()
    {
        var exception = new AccountNotFoundException("details");

        exception.Message.Should().Be("details");
        exception.Should().BeAssignableTo<Exception>();
    }
}
