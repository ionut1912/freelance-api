using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Queries;
using Freelance.UserProfiles.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Shared.Api.Extensions;
using Shared.Api.Infrastructure;
using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Api.Endpoints;

public class ClientProfileEndpointGroup : EndpointGroup
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(this);

        group.MapPost(CreateClientProfile);
        group.MapGet(GetClientProfiles);
        group.MapGet(GetLoggedInClientProfile, "/current");
        group.MapPut(UpdateClientProfileAddress, "/{id:guid}/address");
        group.MapPut(UpdateClientProfileData, "/{id:guid}/data");
        group.MapPut(VerifyClientProfile, "/verify");
        group.MapDelete(DeleteClientProfile, "/{id:guid}");
    }

    [Authorize(Policy = "ClientOnly")]
    private static async Task<IResult> CreateClientProfile(IMediator mediator,
        CreateClientProfileRequest request,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return Results.Unauthorized();

        var command = request.ToCreateCommand(accountId);
        var created = await mediator.Send(command, ct);
        return Results.Created($"/api/client-profiles/{created.Id}", created);
    }

    [Authorize]
    private static async Task<IResult> GetClientProfiles(IMediator mediator, CancellationToken ct)
    {
        var query = new GetClientProfilesQuery();
        var profiles = await mediator.Send(query, ct);
        return Results.Ok(profiles);
    }

    [Authorize(Policy = "ClientOnly")]
    private static async Task<IResult> GetLoggedInClientProfile(IMediator mediator, HttpContext httpContext, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return Results.Unauthorized();

        var query = new GetLoggedInClientProfileQuery(accountId);
        var profile = await mediator.Send(query, ct);
        return Results.Ok(profile);
    }

    [Authorize(Policy = "ClientOnly")]
    private static async Task<IResult> UpdateClientProfileAddress(IMediator mediator, Guid id,
        UpdateProfileAddressRequest request, CancellationToken ct)
    {
        var command = request.ToUpdateClientAddressCommand(id);
        await mediator.Send(command, ct);
        return Results.NoContent();
    }

    [Authorize(Policy = "ClientOnly")]
    private static async Task<IResult> UpdateClientProfileData(IMediator mediator, Guid id,
        UpdateProfileDataRequest request, CancellationToken ct)
    {
        var command = request.ToUpdateClientDataCommand(id);
        await mediator.Send(command, ct);
        return Results.NoContent();
    }

    [Authorize(Policy = "ClientOnly")]
    private static async Task<IResult> DeleteClientProfile(IMediator mediator, Guid id, CancellationToken ct)
    {
        var command = new DeleteClientProfileCommand(id);
        await mediator.Send(command, ct);
        return Results.NoContent();
    }

    [Authorize(Policy = "ClientOnly")]
    private static async Task<IResult> VerifyClientProfile(IMediator mediator,VerifyProfileRequest verifyProfileRequest, HttpContext httpContext, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return Results.Unauthorized();
        var command = verifyProfileRequest.ToVerifyClientCommand(accountId);
        await mediator.Send(command, ct);
        return Results.NoContent();
    }
}