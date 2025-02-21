using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Application.Mediatr.Queries.ClientProfiles;
using Frelance.Contracts.Requests.ClientProfile;
using Frelance.Contracts.Requests.Common;
using Frelance.Web.Extensions;
using Mapster;
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
                    var result = await mediator.Send(createClientProfileRequest.Adapt<CreateClientProfileCommand>(),
                        ct);
                    return Results.Ok(result);
                })
            .WithTags("ClientProfiles")
            .RequireAuthorization("ClientRole");
        app.MapGet("/api/clientProfiles/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
        {
            var clientProfile = await mediator.Send(new GetClientProfileByIdQuery(id), ct);
            return Results.Ok(clientProfile);
        }).WithTags("ClientProfiles").RequireAuthorization();
        app.MapGet("/api/clientProfiles",
            async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken ct) =>
            {
                var paginatedClientProfiles = await mediator.Send(new GetClientProfilesQuery
                    (new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
                return Results.Extensions.OkPaginationResult(paginatedClientProfiles.PageSize,
                    paginatedClientProfiles.CurrentPage,
                    paginatedClientProfiles.TotalCount, paginatedClientProfiles.TotalPages,
                    paginatedClientProfiles.Items);
            }).WithTags("ClientProfiles").RequireAuthorization();
        app.MapGet("/api/current/clientProfiles",
                async (IMediator mediator, CancellationToken ct) =>
                {
                    var clientProfile = await mediator.Send(new GetLoggedInClientProfileQuery(), ct);
                    return Results.Ok(clientProfile);
                }).WithTags("ClientProfiles")
            .RequireAuthorization("ClientRole");
        app.MapPut("/api/clientProfiles/{id}", async (IMediator mediator, int id,
            UpdateClientProfileRequest updateClientProfileRequest, CancellationToken ct) =>
        {
            var command = updateClientProfileRequest.Adapt<UpdateClientProfileCommand>() with { Id = id };
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithTags("ClientProfiles").RequireAuthorization("ClientRole");
        app.MapDelete("/api/clientProfiles/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
        {
            var command = new DeleteClientProfileCommand(id);
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithTags("ClientProfiles").RequireAuthorization("ClientRole");
    }
}