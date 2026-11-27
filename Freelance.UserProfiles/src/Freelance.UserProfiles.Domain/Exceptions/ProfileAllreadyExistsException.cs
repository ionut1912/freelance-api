namespace Freelance.UserProfiles.Domain.Exceptions;

public sealed class ProfileAllreadyExistsException(string message) : Exception(message)
{
}
