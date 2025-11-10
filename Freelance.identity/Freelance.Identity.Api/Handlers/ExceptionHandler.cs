using Freelance.Identity.Domain.Exceptions;
using Freelance.Shared.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Freelance.Identity.Api.Handlers;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = CreateProblemDetails(exception);

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    private static ProblemDetails CreateProblemDetails(Exception exception)
    {
        var problemDetails = exception switch
        {
            AccountAlreadyBlockedException => CreateProblemDetails(StatusCodes.Status400BadRequest,
                "Account Already Blocked", exception.Message),
            AccountNotBlockedException=>CreateProblemDetails(StatusCodes.Status400BadRequest,
                "Account Not Blocked", exception.Message),
            AccountNotFoundException=>CreateProblemDetails(StatusCodes.Status404NotFound,
                "Account Not Found", exception.Message),
            PasswordNotMatchException=>CreateProblemDetails(StatusCodes.Status400BadRequest,
                "Password Not Match", exception.Message),
            UserAlreadyExistsException=>CreateProblemDetails(StatusCodes.Status400BadRequest,
                "User Already Exists", exception.Message),
            CustomValidationException => CreateProblemDetails(StatusCodes.Status400BadRequest,
                "Validation error", "One or more validation errors occurred"),
            _ => CreateProblemDetails(StatusCodes.Status500InternalServerError,
                "Internal Server Error", "An unexpected error occurred")
        };

        if (exception is CustomValidationException customValidationException)
            problemDetails.Extensions["errors"] = customValidationException.ValidationErrors;

        return problemDetails;
    }

    private static ProblemDetails CreateProblemDetails(int status, string title, string detail)
    {
        return new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail
        };
    }
}