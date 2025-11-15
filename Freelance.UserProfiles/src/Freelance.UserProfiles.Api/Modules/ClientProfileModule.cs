using Freelance.UserProfiles.Api.Modules.Extensions;
using Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelancer.UserProfiles.Application.Mediatr.ClientProfiles.Queries;
using Freelancer.UserProfiles.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Freelance.UserProfiles.Api.Modules;

public static class ClientProfileModule
{
    public static void AddClientProfileEndpoints(this IEndpointRouteBuilder app)
    {
        var clientOnlyGroup = app.MapGroup("/api/client-profiles")
            .WithTags("ClientProfile")
            .RequireAuthorization("ClientOnly");
        var authenticatedGroup = app.MapGroup("/api/client-profiles")
            .WithTags("ClientProfile")
            .RequireAuthorization();

        clientOnlyGroup.MapPost("/",
            async (IMediator mediator, CreateClientProfileRequest request, HttpContext httpContext,
                CancellationToken ct) =>
            {
                var accountId = httpContext.GetAccountId();
                if (accountId == Guid.Empty) return Results.Unauthorized();

                var command = new CreateClientProfileCommand
                {
                    AccountId = accountId,
                    Address = request.Address,
                    Bio = request.Bio,
                    Image = request.Image
                };

                var created = await mediator.Send(command, ct);
                return Results.Created($"/api/client-profiles/{created.Id}", created);
            });

        authenticatedGroup.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetClientProfilesQuery(), ct);
            return Results.Ok(result);
        });

        clientOnlyGroup.MapGet("/current", async (IMediator mediator, HttpContext httpContext, CancellationToken ct) =>
        {
            var accountId = httpContext.GetAccountId();
            if (accountId == Guid.Empty) return Results.Unauthorized();

            var query = new GetLoggedInClientProfileQuery { AccountId = accountId };
            var profile = await mediator.Send(query, ct);

            return Results.Ok(profile);
        });

        clientOnlyGroup.MapPut("/{id:guid}/address",
            async (IMediator mediator, Guid id, [FromBody] UpdateProfileAddressRequest request, CancellationToken ct) =>
            {
                var command = new UpdateClientProfileAddressCommand
                {
                    Id = id,
                    AddressDto = request.AddressDto
                };

                await mediator.Send(command, ct);
                return Results.NoContent();
            });

        clientOnlyGroup.MapPut("/{id:guid}/data",
            async (IMediator mediator, Guid id, [FromBody] UpdateProfileDataRequest request, CancellationToken ct) =>
            {
                var command = new UpdateClientProfileDataCommand
                {
                    Id = id,
                    Bio = request.Bio,
                    Image = request.Image
                };

                await mediator.Send(command, ct);
                return Results.NoContent();
            });

        clientOnlyGroup.MapDelete("/{id:guid}", async (IMediator mediator, Guid id, CancellationToken ct) =>
        {
            var command = new DeleteClientProfileCommand { Id = id };
            await mediator.Send(command, ct);
            return Results.NoContent();
        });
    }
}