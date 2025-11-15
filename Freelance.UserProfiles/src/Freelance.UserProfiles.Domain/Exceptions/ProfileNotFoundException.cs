namespace Freelance.UserProfiles.Domain.Exceptions;

public class ProfileNotFoundException(string message) : Exception(message)
{
}