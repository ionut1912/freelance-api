using Frelance.Contracts.Errors;

namespace Frelance.Contracts.Exceptions;

public class CustomValidationException(List<ValidationError> validationErrors) : Exception
{
    public List<ValidationError> ValidationErrors { get; } = validationErrors;
}