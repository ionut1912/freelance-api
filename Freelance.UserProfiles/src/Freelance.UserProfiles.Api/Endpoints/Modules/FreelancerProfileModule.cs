using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;
using Freelance.UserProfiles.Application.Requests;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Extensions;
using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Api.Endpoints.Modules;

public static class FreelancerProfileModule
{
    public static IEndpointRouteBuilder AddFreelancerProfileEndpoints(this IEndpointRouteBuilder app)
    {
        var clientOnlyGroup = app.MapGroup("/api/freelancer-profiles")
            .WithTags("FreelancerProfile")
            .RequireAuthorization("ClientOnly");
        var freelancerOnlyGroup = app.MapGroup("/api/freelancer-profiles")
            .WithTags("FreelancerProfile")
            .RequireAuthorization("FreelancerOnly");
        var authenticatedGroup = app.MapGroup("/api/freelancer-profiles")
            .WithTags("FreelancerProfile")
            .RequireAuthorization();

        freelancerOnlyGroup.MapPost("/", CreateFreelancerProfileAsync);
        authenticatedGroup.MapGet("/", GetFreelancersProfilesAsync);
        freelancerOnlyGroup.MapGet("/current", GetCurrentFreelancerProfileAsync);
        freelancerOnlyGroup.MapPut("/details/{id:guid}", UpdateFreelancerDetailsAsync);
        freelancerOnlyGroup.MapPut("/address/{id:guid}", UpdateFreelancerAddressAsync);
        freelancerOnlyGroup.MapPut("/data/{id:guid}", UpdateFreelancerDataAsync);
        clientOnlyGroup.MapPut("/rating/{id:guid}", UpdateFreelancerRatingAsync);
        freelancerOnlyGroup.MapDelete("/{id:guid}", DeleteFreelancerProfileAsync);

        return app;
    }

    private static async Task<IResult> CreateFreelancerProfileAsync(IMediator mediator, HttpContext httpContext,
                CreateFreelancerProfileRequest createFreelancerProfileRequest, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return Results.Unauthorized();
        var createFreelancerProfileCommand = createFreelancerProfileRequest.ToCreateCommand(accountId);
        var createdFreelancerProfile = await mediator.Send(createFreelancerProfileCommand, ct);
        return Results.Created($"/api/freelancer-profiles/{createdFreelancerProfile.Id}",
            createdFreelancerProfile);
    }

    private static async Task<IResult> GetFreelancersProfilesAsync(IMediator mediator, CancellationToken ct)
    {
        var freelancers = await mediator.Send(new GetFreelancerProfilesQuery(), ct);
        return Results.Ok(freelancers);
    }

    private static async Task<IResult> GetCurrentFreelancerProfileAsync(IMediator mediator, HttpContext httpContext, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return Results.Unauthorized();
        var getCurrentFreelancerQuery = new GetLoggedInFreelancerProfileQuery(accountId);
        var currentFreelancer = await mediator.Send(getCurrentFreelancerQuery, ct);
        return Results.Ok(currentFreelancer);
    }

    private static async Task<IResult> UpdateFreelancerDetailsAsync(IMediator mediator, Guid id, [FromBody] UpdateFreelancerDetailRequest updateFreelancerDetailRequest,
                CancellationToken ct)
    {
        var updateFreelancerDetailsCommand = updateFreelancerDetailRequest.ToUpdateDetailsCommand(id);
        await mediator.Send(updateFreelancerDetailsCommand, ct);
        return Results.NoContent();

    }

    private static async Task<IResult> UpdateFreelancerAddressAsync(IMediator mediator, Guid id, [FromBody] UpdateProfileAddressRequest updateProfileAddressRequest,
        CancellationToken ct)
    {
        var updateFreelancerAddressCommand = updateProfileAddressRequest.ToUpdateFreelancerAddressCommand(id);
        await mediator.Send(updateFreelancerAddressCommand, ct);
        return Results.NoContent();
    }

    private static async Task<IResult> UpdateFreelancerDataAsync(IMediator mediator, Guid id, [FromBody] UpdateProfileDataRequest updateProfileDataRequest,
        CancellationToken ct)
    {
        var updateFreelancerDataCommand = updateProfileDataRequest.ToUpdateFreelancerDataCommand(id);
        await mediator.Send(updateFreelancerDataCommand, ct);
        return Results.NoContent();
    }

    private static async Task<IResult> UpdateFreelancerRatingAsync(IMediator mediator, Guid id, [FromBody] UpdateFreelancerProfileRatingRequest updateFreelancerProfileRatingRequest,
        CancellationToken ct)
    {
        var updateFreelancerRatingCommand = updateFreelancerProfileRatingRequest.ToUpdateRatingCommand(id);
        await mediator.Send(updateFreelancerRatingCommand, ct);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteFreelancerProfileAsync(IMediator mediator, Guid id, CancellationToken ct)
    {
        var deleteFreelancerProfileCommand = new DeleteFreelancerProfileCommand(id);
        await mediator.Send(deleteFreelancerProfileCommand, ct);
        return Results.NoContent();
    }
}