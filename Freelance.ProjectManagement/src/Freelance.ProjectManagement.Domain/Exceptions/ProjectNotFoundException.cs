
namespace Freelance.ProjectManagement.Domain.Exceptions;

public sealed class ProjectNotFoundException(string message) : Exception(message)
{
}
