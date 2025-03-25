using Frelance.Application.Mediatr.Commands.UserProfile;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Requests.ClientProfile;
using MediatR;

namespace Frelance.Web.Modules;

public static class ClientProfileModule
{
    public static void AddClientProfileEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/clientProfiles",
                async (IMediator mediator, CreateClientProfileRequest createClientProfileRequest,
                    CancellationToken ct) =>
                {
                    await mediator.Send(new CreateUserProfileCommand(Role.Client, createClientProfileRequest),
                        ct);
                    return Results.Created();
                })
            .WithTags("ClientProfiles")
            .RequireAuthorization("ClientRole");
    }
}