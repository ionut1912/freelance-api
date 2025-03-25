using Frelance.Application.Mediatr.Commands.UserProfile;
using Frelance.Application.Mediatr.Queries.UserProfile;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Common;
using Frelance.Web.Extensions;
using Frelance.Web.Modules.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class UserProfileModule
{
    public static void AddUserProfileEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/userProfiles/{id:int}",
                async (IMediator mediator, int id, HttpContext httpContext, CancellationToken ct) =>
                {
                    var commandRole = httpContext.GetRole();
                    var result = await mediator.Send(new GetUserProfileByIdQuery(commandRole, id), ct);
                    return Results.Ok(result);
                }).WithTags("UserProfiles")
            .RequireAuthorization();

        app.MapGet("/api/userProfiles",
                async (IMediator mediator, HttpContext httpContext, [FromQuery] int pageSize,
                    [FromQuery] int pageNumber,
                    CancellationToken ct) =>
                {
                    var commandRole = httpContext.GetRole();
                    var paginatedResult =
                        await mediator.Send(
                            new GetUserProfilesQuery(commandRole,
                                new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
                    return Results.Extensions.OkPaginationResult(paginatedResult.PageSize,
                        paginatedResult.CurrentPage,
                        paginatedResult.TotalCount, paginatedResult.TotalPages,
                        paginatedResult.Items);
                }).WithTags("UserProfiles")
            .RequireAuthorization();

        app.MapGet("/api/current/userProfiles",
                async (IMediator mediator, HttpContext httpContext, CancellationToken ct) =>
                {
                    var result = await mediator.Send(new GetCurrentUserProfileQuery(httpContext.GetRole()),
                        ct);
                    return Results.Ok(result);
                }).WithTags("UserProfiles")
            .RequireAuthorization();

        app.MapPatch("/api/userProfiles/address/{id:int}",
                async (IMediator mediator, int id, [FromBody] AddressDto addressDto, HttpContext httpContext,
                    CancellationToken ct) =>
                {
                    await mediator.Send(new PatchAddressCommand(httpContext.GetRole(), id, addressDto), ct);
                    return Results.NoContent();
                }).WithTags("UserProfiles")
            .RequireAuthorization();

        app.MapPatch("/api/userProfiles/userDetails/{id:int}", async (IMediator mediator, int id,
                HttpContext httpContext,
                [FromBody] UserDetailsDto userDetailsDto, CancellationToken ct) =>
            {
                await mediator.Send(new PatchUserDetailsCommand(httpContext.GetRole(), id, userDetailsDto), ct);
                return Results.NoContent();
            }).WithTags("UserProfiles")
            .RequireAuthorization();

        app.MapPatch("/api/userProfiles/verify/{id:int}",
                async (IMediator mediator, HttpContext httpContext, int id, CancellationToken ct) =>
                {
                    await mediator.Send(new VerifyUserProfileCommand(id, httpContext.GetRole()), ct);
                    return Results.NoContent();
                }).WithTags("UserProfiles")
            .RequireAuthorization();

        app.MapDelete("/api/userProfiles/{id:int}",
                async (IMediator mediator, HttpContext httpContext, int id, CancellationToken ct) =>
                {
                    await mediator.Send(new DeleteUserProfileCommand(httpContext.GetRole(), id), ct);
                    return Results.NoContent();
                }).WithTags("UserProfiles")
            .RequireAuthorization();
    }
}