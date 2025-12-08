using Freelance.UserProfiles.Api.Endpoints.Modules;

namespace Freelance.UserProfiles.Api.Endpoints;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapUserProfileEndpoints(this IEndpointRouteBuilder app)
    {
        app.AddClientProfileEndpoints();
        app.AddFreelancerProfileEndpoints();
        return app;
    }
}
