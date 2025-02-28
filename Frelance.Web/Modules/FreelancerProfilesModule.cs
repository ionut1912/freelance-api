using System.Security.Claims;
using Frelance.Application.Mediatr.Commands.UserProfile;
using Frelance.Application.Mediatr.Queries.UserProfile;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Requests.FreelancerProfiles;
using Frelance.Web.Extensions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class FreelancerProfilesModule
{
    public static void AddFreelancerProfilesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/freelancerProfiles",
                async (IMediator mediator, CreateFreelancerProfileRequest createFreelancerProfileRequest,
                    HttpContext httpContext,
                    CancellationToken ct) =>
                {
                    var role=httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                    var result = role switch
                    {
                        "Freelancer" => await mediator.Send(new CreateUserProfileCommand(Role.Freelancer,createFreelancerProfileRequest), ct),
                        _ => throw new InvalidOperationException("Invalid role.")
                    };
                    return Results.Ok(result);
                })
            .WithTags("FreelancerProfiles")
            .RequireAuthorization("FreelancerRole");
        app.MapGet("/api/freelancerProfiles/{id}", async (IMediator mediator,HttpContext httpContext, int id, CancellationToken ct) =>
        {
            var role=httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var result = role switch
            {
                "Freelancer" => await mediator.Send(new GetUserProfileByIdQuery(Role.Freelancer,id), ct),
                _ => throw new InvalidOperationException("Invalid role.")
            };
            return Results.Ok(result);
        }).WithTags("FreelancerProfiles").RequireAuthorization();
        app.MapGet("/api/freelancerProfiles", async (IMediator mediator, [FromQuery] int pageSize,
            HttpContext httpContext,
            [FromQuery] int pageNumber, CancellationToken ct) =>
        {
            var role=httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var paginatedResult = role switch
            {
                "Freelancer" => await mediator.Send(new GetUserProfilesQuery(Role.Freelancer,new PaginationParams{PageSize = pageSize,PageNumber = pageNumber}), ct),
                _ => throw new InvalidOperationException("Invalid role.")
            };

            return Results.Extensions.OkPaginationResult(paginatedResult.PageSize,
                paginatedResult.CurrentPage,
                paginatedResult.TotalCount, paginatedResult.TotalPages,
                paginatedResult.Items);
        }).WithTags("FreelancerProfiles").RequireAuthorization();

        app.MapGet("/api/current/freelancerProfiles",
                async (IMediator mediator,HttpContext httpContext, CancellationToken ct) =>
                {
                    var role=httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                    var result = role switch
                    {
                        "Freelancer" => await mediator.Send(new GetCurrentUserProfileQuery(Role.Freelancer), ct),
                        _ => throw new InvalidOperationException("Invalid role.")
                    };
                    return Results.Ok(result);
                }).WithTags("FreelancerProfiles")
            .RequireAuthorization("FreelancerRole");

        app.MapPut("/api/freelancerProfiles/{id}", async (IMediator mediator, int id,
            HttpContext httpContext,
            UpdateFreelancerProfileRequest updateFreelancerProfileRequest, CancellationToken ct) =>
        {
            var role=httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var result = role switch
            {
                "Freelancer" => await mediator.Send(new UpdateUserProfileCommand(id,Role.Freelancer,updateFreelancerProfileRequest), ct),
                _ => throw new InvalidOperationException("Invalid role.")
            };
            return Results.Ok(result);
        }).WithTags("FreelancerProfiles").RequireAuthorization("FreelancerRole");
        app.MapDelete("/api/freelancerProfiles/{id}", async (IMediator mediator, HttpContext httpContext,int id, CancellationToken ct) =>
        { var role=httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var result = role switch
            {
                "Freelancer" => await mediator.Send(new DeleteUserProfileCommand(Role.Freelancer,id), ct),
                _ => throw new InvalidOperationException("Invalid role.")
            };
            return Results.Ok(result);
        }).WithTags("FreelancerProfiles").RequireAuthorization("FreelancerRole");
    }
}