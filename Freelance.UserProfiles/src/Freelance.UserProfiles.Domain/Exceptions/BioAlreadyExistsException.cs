namespace Freelance.UserProfiles.Domain.Exceptions;

public sealed class BioAlreadyExistsException(string message) : Exception(message)
{
}