using FluentValidation;
using Freelance.Identity.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
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
            // Two concurrent registrations can both pass the lookup; the unique username index stops the second.
            DbUpdateException { InnerException: PostgresException { SqlState: PostgresErrorCodes.UniqueViolation } } =>
                Create(StatusCodes.Status400BadRequest, "User Already Exists", "Account with this username already exists"),
            ValidationException ex => CreateValidation(ex.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage))),
            CustomValidationException ex => CreateValidation(ex.ValidationErrors),
            // Thrown for unreadable requests (bad JSON, wrong types): the client's fault, with its own status code.
            BadHttpRequestException ex => Create(ex.StatusCode, "Bad Request", "The request could not be read."),
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
