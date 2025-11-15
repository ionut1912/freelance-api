namespace Freelance.Identity.Domain.Exceptions;

public class PasswordNotMatchException(string message) : Exception(message);