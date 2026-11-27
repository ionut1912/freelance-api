using FluentValidation;
using Freelance.Identity.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Abstractions;
using Shared.Domain.Common;
using Shared.Domain.Exceptions;

namespace Freelance.Identity.Api.Mappers;

/// <summary>Translates Identity domain exceptions into RFC 9457 problem details.</summary>
public sealed class IdentityExceptionMapper : IExceptionProblemDetailsMapper
{
    public bool TryMap(Exception exception, out ProblemDetails problemDetails)
    {
        ArgumentNullException.ThrowIfNull(exception);

        problemDetails = exception switch
        {
            AccountNotFoundException ex => Create(StatusCodes.Status404NotFound, "Account Not Found", ex.Message),
            AccountAlreadyBlockedException ex => Create(StatusCodes.Status400BadRequest, "Account Already Blocked", ex.Message),
            AccountNotBlockedException ex => Create(StatusCodes.Status400BadRequest, "Account Not Blocked", ex.Message),
            PasswordNotMatchException ex => Create(StatusCodes.Status400BadRequest, "Password Not Match", ex.Message),
            AccountBlockedException ex => Create(StatusCodes.Status400BadRequest, "Account Blocked", ex.Message),
            UserAlreadyExistsException ex => Create(StatusCodes.Status400BadRequest, "User Already Exists", ex.Message),
            ValidationException ex => CreateValidation(ex.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage))),
            CustomValidationException ex => CreateValidation(ex.ValidationErrors),
            _ => Create(StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred")
        };

        return true;
    }

    private static ProblemDetails Create(int status, string title, string detail) =>
        new() { Status = status, Title = title, Detail = detail };

    private static ProblemDetails CreateValidation(IEnumerable<ValidationError> errors)
    {
        var problem = Create(StatusCodes.Status400BadRequest, "Validation Error", "One or more validation errors occurred.");
        problem.Extensions["errors"] = errors.ToList();
        return problem;
    }
}
