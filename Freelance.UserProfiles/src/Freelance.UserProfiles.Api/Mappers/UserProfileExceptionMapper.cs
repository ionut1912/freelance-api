using FluentValidation;
using Freelance.UserProfiles.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Abstractions;
using Shared.Domain.Common;
using Shared.Domain.Exceptions;

namespace Freelance.UserProfiles.Api.Mappers;

/// <summary>Translates UserProfiles domain exceptions into RFC 9457 problem details.</summary>
public sealed class UserProfileExceptionMapper : IExceptionProblemDetailsMapper
{
    public bool TryMap(Exception exception, out ProblemDetails problemDetails)
    {
        ArgumentNullException.ThrowIfNull(exception);

        problemDetails = exception switch
        {
            ProfileNotFoundException ex => Create(StatusCodes.Status404NotFound, "Profile Not Found", ex.Message),
            BioAlreadyExistsException ex => Create(StatusCodes.Status400BadRequest, "Bio Already Exists", ex.Message),
            ImageAlreadyExistsException ex => Create(StatusCodes.Status400BadRequest, "Image Already Exists", ex.Message),
            ProfileAllreadyExistsException ex => Create(StatusCodes.Status400BadRequest, "Profile Already Exists", ex.Message),
            ValidationException ex => CreateValidation(ex.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage))),
            CustomValidationException ex => CreateValidation(ex.ValidationErrors),
            // Thrown for unreadable requests (bad JSON, wrong types): the client's fault, with its own status code.
            BadHttpRequestException ex => Create(ex.StatusCode, "Bad Request", "The request could not be read."),
            _ => Create(StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred.")
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
