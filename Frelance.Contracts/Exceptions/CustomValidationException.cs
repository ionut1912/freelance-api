using Frelance.API.Frelance.Contracts.Errors;

namespace Frelance.API.Frelance.Contracts.Exceptions;

public class CustomValidationException : Exception
{
    public CustomValidationException(List<ValidationError> validationErrors)
    {
        ValidationErrors = validationErrors;
    }
    
    public List<ValidationError> ValidationErrors { get; set; }
}