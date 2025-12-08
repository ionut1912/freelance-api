using Freelance.UserProfiles.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Abstractions;
using Shared.Domain.Exceptions;

namespace Freelance.UserProfiles.Api.Mappers;

public class UserProfileExceptionMapper : IExceptionProblemDetailsMapper
{
    public bool TryMap(Exception exception, out ProblemDetails problemDetails)
    {
        problemDetails = exception switch
        {
            BioAlreadyExistsException ex => Create(400, "Bio Already Exists", ex.Message),
            ImageAlreadyExistsException ex => Create(400, "Image Already Exists", ex.Message),
            ProfileNotFoundException ex => Create(404, "Profile Not Found", ex.Message),

            CustomValidationException ex => CreateValidation(ex),

            _ => Create(500, "Internal Server Error", "An unexpected error occurred.")
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