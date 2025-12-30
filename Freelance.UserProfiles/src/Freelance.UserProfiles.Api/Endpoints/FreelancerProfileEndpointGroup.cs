using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;
using Freelance.UserProfiles.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Extensions;
using Shared.Api.Infrastructure;
using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Api.Endpoints;

public class FreelancerProfileEndpointGroup : EndpointGroup
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(this);

        group.MapPost(CreateFreelancerProfileAsync);
        group.MapGet(GetFreelancersProfilesAsync);
        group.MapGet(GetCurrentFreelancerProfileAsync, "/current");
        group.MapPut(UpdateFreelancerDetailsAsync, "/details/{id:guid}");
        group.MapPut(UpdateFreelancerAddressAsync, "/address/{id:guid}");
        group.MapPut(UpdateFreelancerDataAsync, "/data/{id:guid}");
        group.MapPut(UpdateFreelancerRatingAsync, "/rating/{id:guid}");
        group.MapDelete(DeleteFreelancerProfileAsync, "/{id:guid}");
    }

    [Authorize(Policy = "FreelancerOnly")]
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

    [Authorize]
    private static async Task<IResult> GetFreelancersProfilesAsync(IMediator mediator, CancellationToken ct)
    {
        var freelancers = await mediator.Send(new GetFreelancerProfilesQuery(), ct);
        return Results.Ok(freelancers);
    }

    [Authorize(Policy = "FreelancerOnly")]
    private static async Task<IResult> GetCurrentFreelancerProfileAsync(IMediator mediator, HttpContext httpContext, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return Results.Unauthorized();

        var getCurrentFreelancerQuery = new GetLoggedInFreelancerProfileQuery(accountId);
        var currentFreelancer = await mediator.Send(getCurrentFreelancerQuery, ct);
        return Results.Ok(currentFreelancer);
    }

    [Authorize(Policy = "FreelancerOnly")]
    private static async Task<IResult> UpdateFreelancerDetailsAsync(IMediator mediator, Guid id,
        [FromBody] UpdateFreelancerDetailRequest updateFreelancerDetailRequest, CancellationToken ct)
    {
        var updateFreelancerDetailsCommand = updateFreelancerDetailRequest.ToUpdateDetailsCommand(id);
        await mediator.Send(updateFreelancerDetailsCommand, ct);
        return Results.NoContent();
    }

    [Authorize(Policy = "FreelancerOnly")]
    private static async Task<IResult> UpdateFreelancerAddressAsync(IMediator mediator, Guid id,
        [FromBody] UpdateProfileAddressRequest updateProfileAddressRequest, CancellationToken ct)
    {
        var updateFreelancerAddressCommand = updateProfileAddressRequest.ToUpdateFreelancerAddressCommand(id);
        await mediator.Send(updateFreelancerAddressCommand, ct);
        return Results.NoContent();
    }

    [Authorize(Policy = "FreelancerOnly")]
    private static async Task<IResult> UpdateFreelancerDataAsync(IMediator mediator, Guid id,
        [FromBody] UpdateProfileDataRequest updateProfileDataRequest, CancellationToken ct)
    {
        var updateFreelancerDataCommand = updateProfileDataRequest.ToUpdateFreelancerDataCommand(id);
        await mediator.Send(updateFreelancerDataCommand, ct);
        return Results.NoContent();
    }

    [Authorize(Policy = "ClientOnly")]
    private static async Task<IResult> UpdateFreelancerRatingAsync(IMediator mediator, Guid id,
        [FromBody] UpdateFreelancerProfileRatingRequest updateFreelancerProfileRatingRequest, CancellationToken ct)
    {
        var updateFreelancerRatingCommand = updateFreelancerProfileRatingRequest.ToUpdateRatingCommand(id);
        await mediator.Send(updateFreelancerRatingCommand, ct);
        return Results.NoContent();
    }

    [Authorize(Policy = "FreelancerOnly")]
    private static async Task<IResult> DeleteFreelancerProfileAsync(IMediator mediator, Guid id, CancellationToken ct)
    {
        var deleteFreelancerProfileCommand = new DeleteFreelancerProfileCommand(id);
        await mediator.Send(deleteFreelancerProfileCommand, ct);
        return Results.NoContent();
    }
}