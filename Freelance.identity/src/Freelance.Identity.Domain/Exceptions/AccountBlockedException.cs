namespace Freelance.Identity.Domain.Exceptions;

public class AccountBlockedException(string message) : Exception(message)
{
}