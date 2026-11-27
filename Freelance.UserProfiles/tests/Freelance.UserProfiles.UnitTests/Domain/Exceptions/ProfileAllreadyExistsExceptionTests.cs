using Freelance.UserProfiles.Domain.Exceptions;

namespace Freelance.UserProfiles.UnitTests.Domain.Exceptions;

public sealed class ProfileAllreadyExistsExceptionTests
{
    [Fact]
    public void Constructor_KeepsMessage_AndIsAnException()
    {
        var exception = new ProfileAllreadyExistsException("details");

        exception.Message.Should().Be("details");
        exception.Should().BeAssignableTo<Exception>();
    }
}
