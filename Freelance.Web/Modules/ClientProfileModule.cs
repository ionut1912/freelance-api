using Freelance.Application.Mediatr.Commands.UserProfile;
using Freelance.Contracts.Enums;
using Freelance.Contracts.Requests.ClientProfile;
using MediatR;

namespace Freelance.Web.Modules;

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