using Freelance.FaceRecognition.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Abstractions;

namespace Freelance.FaceRecognition.Api.Mappers;

public class FaceRecognitionExceptionMapper : IExceptionProblemDetailsMapper
{
    public bool TryMap(Exception exception, out ProblemDetails problemDetails)
    {
        problemDetails = exception switch
        {
            FaceNotHumanException ex => Create(400, "Face Not Human", ex.Message),

            _ => Create(500, "Internal Server Error", "An unexpected error occurred")
        };

        return problemDetails != null;
    }

    private ProblemDetails Create(int status, string title, string detail) =>
        new() { Status = status, Title = title, Detail = detail };
}
