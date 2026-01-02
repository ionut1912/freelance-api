using Shared.Api.Infrastructure;

namespace Freelance.Face.Api.Endpoints;

public class CameraEndpointGroup : EndpointGroup
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(this);
        group.MapPost(CaptureCamera, "/capture/sessions");
    }
    private static async Task<IResult> CaptureCamera()
    {
        var sessionId = Guid.NewGuid().ToString("N");
        var baseUrl = Environment.GetEnvironmentVariable("FRONTEND_BASE_URL");

        if (string.IsNullOrWhiteSpace(baseUrl)) return Results.Problem("FRONTEND_BASE_URL is not set.");

        var deepLink = $"{baseUrl.TrimEnd('/')}/remote-capture?session={sessionId}";
        return Results.Ok(new { sessionId, deepLink });
    }
}
