using Freelance.ProjectManagement.Domain.Exceptions;

namespace Freelance.ProjectManagement.UnitTests.Domain.Exceptions;

public sealed class TimeLogNotFoundExceptionTests
{
    [Fact]
    public void Constructor_KeepsMessage_AndIsAnException()
    {
        var exception = new TimeLogNotFoundException("details");

        exception.Message.Should().Be("details");
        exception.Should().BeAssignableTo<Exception>();
    }
}
