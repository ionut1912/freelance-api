using Shared.Domain.Exceptions;
using Freelance.UserProfiles.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Freelance.UserProfiles.Api.Handlers;

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
            case BioAlreadyExistsException:
                problemDetails = CreateProblemDetails(StatusCodes.Status400BadRequest,
                    "Bio Already Exists", exception.Message);
                break;

            case ImageAlreadyExistsException:
                problemDetails = CreateProblemDetails(StatusCodes.Status400BadRequest,
                    "Image Already Exists", exception.Message);
                break;

            case ProfileNotFoundException:
                problemDetails = CreateProblemDetails(StatusCodes.Status404NotFound,
                    "Profile Not Found", exception.Message);
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