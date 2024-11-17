namespace Frelance.API.Frelamce.Contracts;

public class CustomValidationException : Exception
{
    public CustomValidationException(List<ValidationError> validationErrors)
    {
        ValidationErrors = validationErrors;
    }
    
    public List<ValidationError> ValidationErrors { get; set; }
}