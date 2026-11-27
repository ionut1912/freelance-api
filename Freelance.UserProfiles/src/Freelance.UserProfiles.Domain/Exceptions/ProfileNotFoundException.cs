namespace Freelance.UserProfiles.Domain.Exceptions;

public sealed class ProfileNotFoundException(string message) : Exception(message)
{
}