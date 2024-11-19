using Frelance.Contracts.Errors;

namespace Frelance.Contracts.Exceptions;

public class CustomValidationException : Exception
{
    public CustomValidationException(List<ValidationError> validationErrors)
    {
        ValidationErrors = validationErrors;
    }
    
    public List<ValidationError> ValidationErrors { get; set; }
}