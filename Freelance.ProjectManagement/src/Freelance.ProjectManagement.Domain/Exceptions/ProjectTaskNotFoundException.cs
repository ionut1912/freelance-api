namespace Freelance.ProjectManagement.Domain.Exceptions;

public sealed class ProjectTaskNotFoundException(string message) : Exception(message)
{
}
