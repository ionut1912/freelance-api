namespace Freelance.Web.Modules;

public static class CameraModule
{
    public static void AddCameraEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/capture/sessions", () =>
        {
            var sessionId = Guid.NewGuid().ToString("N");
            var baseUrl = Environment.GetEnvironmentVariable("FRONTEND_BASE_URL");

            if (string.IsNullOrWhiteSpace(baseUrl)) return Results.Problem("FRONTEND_BASE_URL is not set.");

            var deepLink = $"{baseUrl.TrimEnd('/')}/remote-capture?session={sessionId}";
            return Results.Ok(new { sessionId, deepLink });
        });
    }
}