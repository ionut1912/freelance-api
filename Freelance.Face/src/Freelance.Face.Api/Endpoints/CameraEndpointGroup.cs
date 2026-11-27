using Freelance.Face.Api.Options;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
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

    private static Results<Ok<CaptureSessionResponse>, ProblemHttpResult> CreateCaptureSession(
        IOptionsMonitor<FrontendOptions> frontend)
    {
        var baseUrl = frontend.CurrentValue.BaseUrl;
        if (string.IsNullOrWhiteSpace(baseUrl)) return TypedResults.Problem($"{FrontendOptions.BaseUrlKey} is not set.");

        var sessionId = Guid.NewGuid().ToString("N");
        var deepLink = $"{baseUrl.TrimEnd('/')}/remote-capture?session={sessionId}";
        return TypedResults.Ok(new CaptureSessionResponse(sessionId, deepLink));
    }
}

public sealed record CaptureSessionResponse(string SessionId, string DeepLink);
