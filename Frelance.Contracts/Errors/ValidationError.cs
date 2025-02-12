namespace Frelance.Contracts.Errors
{
    public record ValidationError(string Property, string ErrorMessage)
    {
    }
}