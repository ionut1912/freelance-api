using Freelance.ProjectManagement.Domain.Exceptions;

namespace Freelance.ProjectManagement.UnitTests.Domain.Exceptions;

public sealed class ProjectTaskNotFoundExceptionTests
{
    [Fact]
    public void Constructor_KeepsMessage_AndIsAnException()
    {
        var exception = new ProjectTaskNotFoundException("details");

        exception.Message.Should().Be("details");
        exception.Should().BeAssignableTo<Exception>();
    }
}
