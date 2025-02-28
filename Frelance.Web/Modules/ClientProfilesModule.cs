using Frelance.Contracts.Requests.ClientProfile;
using Frelance.Contracts.Requests.Common;
using Frelance.Web.Extensions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Frelance.Application.Mediatr.Commands.UserProfile;
using Frelance.Application.Mediatr.Queries.UserProfile;
using Frelance.Contracts.Enums;

namespace Frelance.Web.Modules;

public static class ClientProfilesModule
{
    public static void AddClientProfilesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/clientProfiles",
                async (IMediator mediator, CreateClientProfileRequest createClientProfileRequest,HttpContext httpContext,
                    CancellationToken ct) =>
                {
                    var role=httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                    var result = role switch
                    {
                        "Client" => await mediator.Send(new CreateUserProfileCommand(Role.Client,createClientProfileRequest), ct),
                        _ => throw new InvalidOperationException("Invalid role.")
                    };
                    return Results.Ok(result);
                })
            .WithTags("ClientProfiles")
            .RequireAuthorization("ClientRole");
        app.MapGet("/api/clientProfiles/{id}", async (IMediator mediator, int id,HttpContext httpContext, CancellationToken ct) =>
        {
            var role=httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var result = role switch
            {
                "Client" => await mediator.Send(new GetUserProfileByIdQuery(Role.Client,id), ct),
                _ => throw new InvalidOperationException("Invalid role.")
            };
            return Results.Ok(result);
        }).WithTags("ClientProfiles").RequireAuthorization();
        app.MapGet("/api/clientProfiles",
            async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber,HttpContext httpContext, CancellationToken ct) =>
            {
                var role=httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                var paginatedResult = role switch
                {
                    "Client" => await mediator.Send(new GetUserProfilesQuery(Role.Client,new PaginationParams{PageSize = pageSize,PageNumber = pageNumber}), ct),
                    _ => throw new InvalidOperationException("Invalid role.")
                };

                return Results.Extensions.OkPaginationResult(paginatedResult.PageSize,
                    paginatedResult.CurrentPage,
                    paginatedResult.TotalCount, paginatedResult.TotalPages,
                    paginatedResult.Items);
            }).WithTags("ClientProfiles").RequireAuthorization();
        app.MapGet("/api/current/clientProfiles",
                async (IMediator mediator,HttpContext httpContext, CancellationToken ct) =>
                {
                var role=httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                var result = role switch
                {
                    "Client" => await mediator.Send(new GetCurrentUserProfileQuery(Role.Client), ct),
                    _ => throw new InvalidOperationException("Invalid role.")
                };
                    return Results.Ok(result);
                }).WithTags("ClientProfiles")
            .RequireAuthorization("ClientRole");
        app.MapPut("/api/clientProfiles/{id}", async (IMediator mediator, int id,
            UpdateClientProfileRequest updateClientProfileRequest, HttpContext httpContext,CancellationToken ct) =>
        {
            var role=httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var result = role switch
            {
                "Client" => await mediator.Send(new UpdateUserProfileCommand(id,Role.Client,updateClientProfileRequest), ct),
                _ => throw new InvalidOperationException("Invalid role.")
            };
            return Results.Ok(result);
        }).WithTags("ClientProfiles").RequireAuthorization("ClientRole");
        app.MapDelete("/api/clientProfiles/{id}", async (IMediator mediator,HttpContext httpContext, int id, CancellationToken ct) =>
        {
            var role=httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var result = role switch
            {
                "Client" => await mediator.Send(new DeleteUserProfileCommand(Role.Client,id), ct),
                _ => throw new InvalidOperationException("Invalid role.")
            };
            return Results.Ok(result);
        }).WithTags("ClientProfiles").RequireAuthorization("ClientRole");
    }
}