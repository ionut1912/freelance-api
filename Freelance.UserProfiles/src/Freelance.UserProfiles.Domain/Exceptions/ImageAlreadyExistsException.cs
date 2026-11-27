namespace Freelance.UserProfiles.Domain.Exceptions;

public sealed class ImageAlreadyExistsException(string message) : Exception(message)
{
}