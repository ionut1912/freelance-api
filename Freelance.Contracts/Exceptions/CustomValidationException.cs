using Freelance.Contracts.Errors;

namespace Freelance.Contracts.Exceptions;

public class CustomValidationException(List<ValidationError> validationErrors) : Exception
{
    public List<ValidationError> ValidationErrors { get; } = validationErrors;
}