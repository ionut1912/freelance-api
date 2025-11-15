namespace Freelance.Identity.Domain.Exceptions;

public class AccountAlreadyBlockedException(string message) : Exception(message);