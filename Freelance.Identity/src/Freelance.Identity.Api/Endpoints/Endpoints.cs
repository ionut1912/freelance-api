using Freelance.Identity.Api.Endpoints.Modules;

namespace Freelance.Identity.Api.Endpoints;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.AddUserEndpoints();
        return app;
    }
}
