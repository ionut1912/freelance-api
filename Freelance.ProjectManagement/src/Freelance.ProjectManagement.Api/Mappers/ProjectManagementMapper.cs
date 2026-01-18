using Freelance.ProjectManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Abstractions;
using Shared.Domain.Exceptions;

namespace Freelance.ProjectManagement.Api.Mappers;

public class ProjectManagementMapper: IExceptionProblemDetailsMapper
{
    private readonly ILogger<ProjectManagementMapper> _logger;

    public ProjectManagementMapper(ILogger<ProjectManagementMapper> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public bool TryMap(Exception exception, out ProblemDetails problemDetails)
    {
        ArgumentNullException.ThrowIfNull(exception);

        problemDetails = exception switch
        {
            ProjectNotFoundException ex => Create(400, "Project Not Found", ex.Message),
            ProjectTaskNotFoundException ex => Create(400, "Project Task Not Found", ex.Message),
            TimeLogNotFoundException ex => Create(404, "Time Log Not Found", ex.Message),
            CustomValidationException ex => CreateValidation(ex),
            _ => Create(500, "Internal Server Error", "An unexpected error occurred")
        };

        _logger.LogError(
            exception,
            "Mapped exception {ExceptionType} to ProblemDetails {Status} - {Title}",
            exception.GetType().Name,
            problemDetails.Status,
            problemDetails.Title);

        return problemDetails != null;
    }

    private static ProblemDetails Create(int status, string title, string detail) =>
        new() { Status = status, Title = title, Detail = detail };

    private static ProblemDetails CreateValidation(CustomValidationException ex)
    {
        var pd = Create(400, "Validation Error", "One or more validation errors occurred.");
        pd.Extensions["errors"] = ex.ValidationErrors;
        return pd;
    }
}
