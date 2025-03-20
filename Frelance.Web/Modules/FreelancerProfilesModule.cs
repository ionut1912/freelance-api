using Frelance.Application.Mediatr.Commands.UserProfile;
using Frelance.Application.Mediatr.Queries.UserProfile;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Requests.FreelancerProfiles;
using Frelance.Web.Extensions;
using Frelance.Web.Modules.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class FreelancerProfilesModule
{
    public static void AddFreelancerProfilesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/freelancerProfiles",
                async (IMediator mediator, CreateFreelancerProfileRequest createFreelancerProfileRequest,
                    CancellationToken ct) =>
                {
                    await mediator.Send(
                        new CreateUserProfileCommand(Role.Freelancer, createFreelancerProfileRequest), ct);
                    return Results.Created();
                })
            .WithTags("FreelancerProfiles")
            .RequireAuthorization("FreelancerRole");
        app.MapGet("/api/freelancerProfiles/{id:int}",
                async (IMediator mediator, HttpContext httpContext, int id, CancellationToken ct) =>
                {
                    var result = await mediator.Send(new GetUserProfileByIdQuery(ModulesUtils.GetRole(httpContext), id),
                        ct);
                    return Results.Ok(result);
                }).WithTags("FreelancerProfiles")
            .RequireAuthorization();
        app.MapGet("/api/freelancerProfiles", async (IMediator mediator, HttpContext httpContext,
                [FromQuery] int pageSize,
                [FromQuery] int pageNumber, CancellationToken ct) =>
            {
                var paginatedResult =
                    await mediator.Send(
                        new GetUserProfilesQuery(ModulesUtils.GetRole(httpContext),
                            new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);

                return Results.Extensions.OkPaginationResult(paginatedResult.PageSize,
                    paginatedResult.CurrentPage,
                    paginatedResult.TotalCount, paginatedResult.TotalPages,
                    paginatedResult.Items);
            }).WithTags("FreelancerProfiles")
            .RequireAuthorization();

        app.MapGet("/api/current/freelancerProfiles",
                async (IMediator mediator, HttpContext httpContext, CancellationToken ct) =>
                {
                    var result = await mediator.Send(new GetCurrentUserProfileQuery(ModulesUtils.GetRole(httpContext)),
                        ct);
                    return Results.Ok(result);
                }).WithTags("FreelancerProfiles")
            .RequireAuthorization();
        app.MapPatch("/api/freelancerProfiles/address/{id:int}",
                async (IMediator mediator, int id, [FromBody] AddressDto addressDto, CancellationToken ct) =>
                {
                    await mediator.Send(new PatchAddressCommand(Role.Freelancer, id, addressDto), ct);
                    return Results.NoContent();
                }).WithTags("FreelancerProfiles")
            .RequireAuthorization("FreelancerRole");

        app.MapPatch("/api/freelancerProfiles/userDetails/{id:int}", async (IMediator mediator, int id,
                [FromBody] UserDetailsDto userDetailsDto, CancellationToken ct) =>
            {
                await mediator.Send(new PatchUserDetailsCommand(Role.Freelancer, id, userDetailsDto), ct);
                return Results.NoContent();
            }).WithTags("FreelancerProfiles")
            .RequireAuthorization("FreelancerRole");

        app.MapPatch("/api/freelancerProfiles/freelancerDetails/{id:int}", async (IMediator mediator, int id,
                [FromBody] FreelancerProfileData freelancerProfileData, CancellationToken ct) =>
            {
                await mediator.Send(new PatchFreelancerDataCommand(id, freelancerProfileData), ct);
                return Results.NoContent();
            }).WithTags("FreelancerProfiles")
            .RequireAuthorization("FreelancerRole");

        app.MapPatch("/api/freelancerProfiles/verify/{id:int}",
                async (IMediator mediator, int id, CancellationToken ct) =>
                {
                    await mediator.Send(new VerifyUserProfileCommand(id, Role.Freelancer),
                        ct);
                    return Results.NoContent();
                }).WithTags("FreelancerProfiles")
            .RequireAuthorization("FreelancerRole");

        app.MapDelete("/api/freelancerProfiles/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                await mediator.Send(new DeleteUserProfileCommand(Role.Freelancer, id), ct);
                return Results.NoContent();
            }).WithTags("FreelancerProfiles")
            .RequireAuthorization("FreelancerRole");
    }
}