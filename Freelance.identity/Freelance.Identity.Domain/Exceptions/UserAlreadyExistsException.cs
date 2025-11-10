namespace Freelance.Identity.Domain.Exceptions;

public class UserAlreadyExistsException(string message) : Exception(message);