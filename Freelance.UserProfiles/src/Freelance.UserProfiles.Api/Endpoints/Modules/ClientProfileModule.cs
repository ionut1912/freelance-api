using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Queries;
using Freelance.UserProfiles.Application.Requests;
using Shared.Api.Extensions;
using Shared.Application.Mediator;

namespace Freelance.UserProfiles.Api.Endpoints.Modules;

public static class ClientProfileModule
{
    public static IEndpointRouteBuilder AddClientProfileEndpoints(this IEndpointRouteBuilder app)
    {
        var clientOnlyGroup = app.MapGroup("/api/client-profiles")
            .WithTags("ClientProfile")
            .RequireAuthorization("ClientOnly");
        var authenticatedGroup = app.MapGroup("/api/client-profiles")
            .WithTags("ClientProfile")
            .RequireAuthorization();

        clientOnlyGroup.MapPost("/", CreateClientProfile);
        authenticatedGroup.MapGet("/", GetClientProfiles);
        clientOnlyGroup.MapGet("/current", GetLoggedInClientProfile);
        clientOnlyGroup.MapPut("/{id:guid}/address", UpdateClientProfileAddress);
        clientOnlyGroup.MapPut("/{id:guid}/data", UpdateClientProfileData);
        clientOnlyGroup.MapDelete("/{id:guid}", DeleteClientProfile);

        return app;
    }

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

    private static async Task<IResult> GetClientProfiles(IMediator mediator, CancellationToken ct)
    {
        var query = new GetClientProfilesQuery();
        var profiles = await mediator.Send(query, ct);
        return Results.Ok(profiles);
    }

    private static async Task<IResult> GetLoggedInClientProfile(IMediator mediator, HttpContext httpContext, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return Results.Unauthorized();
        var query = new GetLoggedInClientProfileQuery(accountId);
        var profile = await mediator.Send(query, ct);
        return Results.Ok(profile);
    }

    private static async Task<IResult> UpdateClientProfileAddress(IMediator mediator, Guid id,
        UpdateProfileAddressRequest request, CancellationToken ct)
    {
        var command = request.ToUpdateClientAddressCommand(id);
        await mediator.Send(command, ct);
        return Results.NoContent();
    }

    private static async Task<IResult> UpdateClientProfileData(IMediator mediator, Guid id,
        UpdateProfileDataRequest request, CancellationToken ct)
    {
        var command = request.ToUpdateClientDataCommand(id);
        await mediator.Send(command, ct);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteClientProfile(IMediator mediator, Guid id, CancellationToken ct)
    {
        var command = new DeleteClientProfileCommand(id);
        await mediator.Send(command, ct);
        return Results.NoContent();
    }
}