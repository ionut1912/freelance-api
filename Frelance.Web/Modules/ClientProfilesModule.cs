using Frelance.Application.Mediatr.Commands.UserProfile;
using Frelance.Application.Mediatr.Queries.UserProfile;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Requests.ClientProfile;
using Frelance.Contracts.Requests.Common;
using Frelance.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class ClientProfilesModule
{
    public static void AddClientProfilesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/clientProfiles",
                async (IMediator mediator, CreateClientProfileRequest createClientProfileRequest,
                    CancellationToken ct) =>
                {
                    await mediator.Send(new CreateUserProfileCommand(Role.Client, createClientProfileRequest), ct);
                    return Results.Created();
                })
            .WithTags("ClientProfiles")
            .RequireAuthorization("ClientRole");
        app.MapGet("/api/clientProfiles/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetUserProfileByIdQuery(Role.Client, id), ct);
                return Results.Ok(result);
            }).WithTags("ClientProfiles")
            .RequireAuthorization("ClientRole");
        app.MapGet("/api/clientProfiles",
                async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber,
                    CancellationToken ct) =>
                {
                    var paginatedResult =
                        await mediator.Send(
                            new GetUserProfilesQuery(Role.Client,
                                new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
                    return Results.Extensions.OkPaginationResult(paginatedResult.PageSize,
                        paginatedResult.CurrentPage,
                        paginatedResult.TotalCount, paginatedResult.TotalPages,
                        paginatedResult.Items);
                }).WithTags("ClientProfiles")
            .RequireAuthorization("ClientRole");
        app.MapGet("/api/current/clientProfiles",
                async (IMediator mediator, CancellationToken ct) =>
                {
                    var result = await mediator.Send(new GetCurrentUserProfileQuery(Role.Client), ct);
                    return Results.Ok(result);
                }).WithTags("ClientProfiles")
            .RequireAuthorization("ClientRole");
        app.MapPut("/api/clientProfiles/{id}", async (IMediator mediator, int id,
                UpdateClientProfileRequest updateClientProfileRequest, CancellationToken ct) =>
            {
                await mediator.Send(new UpdateUserProfileCommand(id, Role.Client, updateClientProfileRequest), ct);
                return Results.NoContent();
            }).WithTags("ClientProfiles")
            .RequireAuthorization("ClientRole");

        app.MapPatch("/api/clientProfiles/verify/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                await mediator.Send(new VerifyUserProfileCommand(id, Role.Client), ct);
                return Results.NoContent();
            }).WithTags("ClientProfiles")
            .RequireAuthorization("ClientRole");

        app.MapDelete("/api/clientProfiles/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                await mediator.Send(new DeleteUserProfileCommand(Role.Client, id), ct);
                return Results.NoContent();
            }).WithTags("ClientProfiles")
            .RequireAuthorization("ClientRole");
    }
}