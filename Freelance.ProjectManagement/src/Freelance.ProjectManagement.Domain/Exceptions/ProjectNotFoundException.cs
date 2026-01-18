
namespace Freelance.ProjectManagement.Domain.Exceptions;

public class ProjectNotFoundException(string message) : Exception(message)
{
}
