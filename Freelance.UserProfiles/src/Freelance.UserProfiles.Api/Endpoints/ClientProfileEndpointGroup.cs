using Freelance.UserProfiles.Application.Dtos;
using Freelance.UserProfiles.Application.Mappings;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Commands;
using Freelance.UserProfiles.Application.Mediatr.ClientProfiles.Queries;
using Freelance.UserProfiles.Application.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Api.Endpoints;
using Shared.Api.Extensions;
using Wolverine;

namespace Freelance.UserProfiles.Api.Endpoints;

public sealed class ClientProfileEndpointGroup : EndpointGroup
{
    public override void Map(IEndpointRouteBuilder app)
    {
        // Explicit prefix: it is part of the public contract routed by the API gateway (/client-profiles/*).
        var group = app.MapGroup("/client-profiles").WithTags("Client Profile").RequireAuthorization();
        var clients = group.MapGroup(string.Empty).RequireAuthorization(AuthorizationPolicies.ClientOnly);

        group.MapGet("", GetClientProfiles).WithName(nameof(GetClientProfiles));

        clients.MapPost("", CreateClientProfile).WithName(nameof(CreateClientProfile));
        clients.MapGet("/current", GetLoggedInClientProfile).WithName(nameof(GetLoggedInClientProfile));
        clients.MapPut("/{id:guid}/address", UpdateClientProfileAddress).WithName(nameof(UpdateClientProfileAddress));
        clients.MapPut("/{id:guid}/data", UpdateClientProfileData).WithName(nameof(UpdateClientProfileData));
        clients.MapPut("/verify", VerifyClientProfile).WithName(nameof(VerifyClientProfile));
        clients.MapDelete("/{id:guid}", DeleteClientProfile).WithName(nameof(DeleteClientProfile));
    }

    private static async Task<Results<Created<ClientProfileDto>, UnauthorizedHttpResult>> CreateClientProfile(
        CreateClientProfileRequest request, HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        var created = await bus.InvokeAsync<ClientProfileDto>(request.ToCreateCommand(accountId), ct);
        return TypedResults.Created($"/client-profiles/{created.Id}", created);
    }

    private static async Task<Ok<List<ClientProfileDto>>> GetClientProfiles(IMessageBus bus, CancellationToken ct) =>
        TypedResults.Ok(await bus.InvokeAsync<List<ClientProfileDto>>(new GetClientProfilesQuery(), ct));

    private static async Task<Results<Ok<ClientProfileDto>, UnauthorizedHttpResult>> GetLoggedInClientProfile(
        HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        return TypedResults.Ok(await bus.InvokeAsync<ClientProfileDto>(new GetLoggedInClientProfileQuery(accountId), ct));
    }

    private static async Task<NoContent> UpdateClientProfileAddress(Guid id, UpdateProfileAddressRequest request,
        IMessageBus bus, CancellationToken ct)
    {
        await bus.InvokeAsync(request.ToUpdateClientAddressCommand(id), ct);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> UpdateClientProfileData(Guid id, UpdateProfileDataRequest request,
        IMessageBus bus, CancellationToken ct)
    {
        await bus.InvokeAsync(request.ToUpdateClientDataCommand(id), ct);
        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeleteClientProfile(Guid id, IMessageBus bus, CancellationToken ct)
    {
        await bus.InvokeAsync(new DeleteClientProfileCommand(id), ct);
        return TypedResults.NoContent();
    }

    /// <summary>Starts an asynchronous face verification; the result arrives later through RabbitMQ.</summary>
    private static async Task<Results<NoContent, UnauthorizedHttpResult>> VerifyClientProfile(
        VerifyProfileRequest request, HttpContext httpContext, IMessageBus bus, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        await bus.InvokeAsync(request.ToVerifyClientCommand(accountId), ct);
        return TypedResults.NoContent();
    }
}
