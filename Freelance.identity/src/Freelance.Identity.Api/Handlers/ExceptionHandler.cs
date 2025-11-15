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
        ProblemDetails problemDetails;
        switch (exception)
        {
            case AccountAlreadyBlockedException:
                problemDetails = CreateProblemDetails(StatusCodes.Status400BadRequest,
                    "Account Already Blocked", exception.Message);
                break;
            case AccountNotBlockedException:
                problemDetails = CreateProblemDetails(StatusCodes.Status400BadRequest,
                    "Account Not Blocked", exception.Message);
                break;
            case AccountNotFoundException:
                problemDetails = CreateProblemDetails(StatusCodes.Status404NotFound,
                    "Account Not Found", exception.Message);
                break;
            case PasswordNotMatchException:
                problemDetails = CreateProblemDetails(StatusCodes.Status400BadRequest,
                    "Password Not Match", exception.Message);
                break;
            case AccountBlockedException:
                problemDetails = CreateProblemDetails(StatusCodes.Status400BadRequest,
                    "Account blocked", exception.Message);
                break;
            case UserAlreadyExistsException:
                problemDetails = CreateProblemDetails(StatusCodes.Status400BadRequest,
                    "User Already Exists", exception.Message);
                break;
            case CustomValidationException validationException:
                problemDetails = CreateProblemDetails(StatusCodes.Status400BadRequest,
                    "Validation Error", "One or more validation errors occurred.");
                problemDetails.Extensions["errors"] = validationException.ValidationErrors;
                break;

            default:
                problemDetails = CreateProblemDetails(StatusCodes.Status500InternalServerError,
                    "Internal Server Error", "An unexpected error occurred");
                break;
        }

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