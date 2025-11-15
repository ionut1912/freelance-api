namespace Freelance.UserProfiles.Domain.Exceptions;

public class BioAlreadyExistsException(string message) : Exception(message)
{
}