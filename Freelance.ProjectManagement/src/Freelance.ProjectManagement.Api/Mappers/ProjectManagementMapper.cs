using FluentValidation;
using Freelance.ProjectManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Abstractions;
using Shared.Domain.Common;
using Shared.Domain.Exceptions;

namespace Freelance.ProjectManagement.Api.Mappers;

/// <summary>Translates ProjectManagement domain exceptions into RFC 9457 problem details.</summary>
public sealed class ProjectManagementMapper : IExceptionProblemDetailsMapper
{
    public bool TryMap(Exception exception, out ProblemDetails problemDetails)
    {
        ArgumentNullException.ThrowIfNull(exception);

        problemDetails = exception switch
        {
            ProjectNotFoundException ex => Create(StatusCodes.Status404NotFound, "Project Not Found", ex.Message),
            ProjectTaskNotFoundException ex => Create(StatusCodes.Status404NotFound, "Project Task Not Found", ex.Message),
            TimeLogNotFoundException ex => Create(StatusCodes.Status404NotFound, "Time Log Not Found", ex.Message),
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
