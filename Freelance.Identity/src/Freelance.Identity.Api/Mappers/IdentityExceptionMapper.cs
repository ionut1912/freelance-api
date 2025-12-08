using Freelance.Identity.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Abstractions;
using Shared.Domain.Exceptions;

namespace Freelance.Identity.Api.Mappers;

public class IdentityExceptionMapper : IExceptionProblemDetailsMapper
{
    public bool TryMap(Exception exception, out ProblemDetails problemDetails)
    {
        problemDetails = exception switch
        {
            AccountAlreadyBlockedException ex => Create(400, "Account Already Blocked", ex.Message),
            AccountNotBlockedException ex => Create(400, "Account Not Blocked", ex.Message),
            AccountNotFoundException ex => Create(404, "Account Not Found", ex.Message),
            PasswordNotMatchException ex => Create(400, "Password Not Match", ex.Message),
            AccountBlockedException ex => Create(400, "Account Blocked", ex.Message),
            UserAlreadyExistsException ex => Create(400, "User Already Exists", ex.Message),

            CustomValidationException ex => CreateValidation(ex),

            _ => Create(500, "Internal Server Error", "An unexpected error occurred")
        };

        return problemDetails != null;
    }

    private ProblemDetails Create(int status, string title, string detail) =>
        new() { Status = status, Title = title, Detail = detail };

    private ProblemDetails CreateValidation(CustomValidationException ex)
    {
        var pd = Create(400, "Validation Error", "One or more validation errors occurred.");
        pd.Extensions["errors"] = ex.ValidationErrors;
        return pd;
    }
}
