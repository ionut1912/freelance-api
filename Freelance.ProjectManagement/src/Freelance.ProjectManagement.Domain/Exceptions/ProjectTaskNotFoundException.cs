namespace Freelance.ProjectManagement.Domain.Exceptions;

public class ProjectTaskNotFoundException(string message) : Exception(message)
{
}
