using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Api.Endpoints;

namespace Freelance.Face.Api.Endpoints;

public sealed class CameraEndpointGroup : EndpointGroup
{
    public override void Map(IEndpointRouteBuilder app)
    {
        // Explicit prefix: it is part of the public contract routed by the API gateway (/camera/*).
        var group = app.MapGroup("/camera").WithTags("Camera");
        group.MapPost("/capture/sessions", CreateCaptureSession).WithName(nameof(CreateCaptureSession));
    }

    private static Results<Ok<CaptureSessionResponse>, ProblemHttpResult> CreateCaptureSession(IConfiguration configuration)
    {
        var baseUrl = configuration["FRONTEND_BASE_URL"];
        if (string.IsNullOrWhiteSpace(baseUrl)) return TypedResults.Problem("FRONTEND_BASE_URL is not set.");

        var sessionId = Guid.NewGuid().ToString("N");
        var deepLink = $"{baseUrl.TrimEnd('/')}/remote-capture?session={sessionId}";
        return TypedResults.Ok(new CaptureSessionResponse(sessionId, deepLink));
    }
}

public sealed record CaptureSessionResponse(string SessionId, string DeepLink);
