using Freelance.Identity.Domain.Exceptions;

namespace Freelance.Identity.UnitTests.Domain.Exceptions;

public sealed class UserAlreadyExistsExceptionTests
{
    [Fact]
    public void Constructor_KeepsMessage_AndIsAnException()
    {
        var exception = new UserAlreadyExistsException("details");

        exception.Message.Should().Be("details");
        exception.Should().BeAssignableTo<Exception>();
    }
}
