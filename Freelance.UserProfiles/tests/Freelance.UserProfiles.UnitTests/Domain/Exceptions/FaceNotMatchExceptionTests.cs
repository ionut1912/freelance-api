using Freelance.UserProfiles.Domain.Exceptions;

namespace Freelance.UserProfiles.UnitTests.Domain.Exceptions;

public sealed class FaceNotMatchExceptionTests
{
    [Fact]
    public void Constructor_KeepsMessage_AndIsAnException()
    {
        var exception = new FaceNotMatchException("details");

        exception.Message.Should().Be("details");
        exception.Should().BeAssignableTo<Exception>();
    }
}
