namespace Freelance.ProjectManagement.Domain.Exceptions;

public sealed class TimeLogNotFoundException(string message) : Exception(message)
{
}
