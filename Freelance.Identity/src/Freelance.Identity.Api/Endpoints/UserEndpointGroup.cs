using System.Security.Claims;
using Freelance.Identity.Application.Dtos;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Mediatr.Accounts.Query;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Api.Endpoints;
using Shared.Api.Extensions;
using Wolverine;

namespace Freelance.Identity.Api.Endpoints;

public sealed class UserEndpointGroup : EndpointGroup
{
    public override void Map(IEndpointRouteBuilder app)
    {
        // Explicit prefix: it is part of the public contract routed by the API gateway (/auth/*).
        var group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("/register", RegisterUser).WithName(nameof(RegisterUser)).AllowAnonymous();
        group.MapPost("/login", LoginUser).WithName(nameof(LoginUser)).AllowAnonymous();

        var authenticated = group.MapGroup(string.Empty).RequireAuthorization();
        authenticated.MapGet("/current", GetCurrentUser).WithName(nameof(GetCurrentUser));
        authenticated.MapPut("/block", BlockUserAccount).WithName(nameof(BlockUserAccount));
        authenticated.MapPut("/unblock", UnblockUserAccount).WithName(nameof(UnblockUserAccount));
        authenticated.MapDelete("/", DeleteUserAccount).WithName(nameof(DeleteUserAccount));
    }

    private static async Task<Created<AccountDto>> RegisterUser(CreateAccountCommand command, IMessageBus bus,
        CancellationToken ct)
    {
        var account = await bus.InvokeAsync<AccountDto>(command, ct);
        return TypedResults.Created($"/auth/{account.Id}", account);
    }

    private static async Task<Ok<AccountDto>> LoginUser(LoginQuery query, IMessageBus bus, CancellationToken ct) =>
        TypedResults.Ok(await bus.InvokeAsync<AccountDto>(query, ct));

    private static async Task<Results<Ok<AccountDto>, UnauthorizedHttpResult>> GetCurrentUser(HttpContext httpContext,
        IMessageBus bus, CancellationToken ct)
    {
        var username = httpContext.User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(username)) return TypedResults.Unauthorized();

        return TypedResults.Ok(await bus.InvokeAsync<AccountDto>(new GetCurrentAccountQuery(username), ct));
    }

    private static Task<Results<NoContent, UnauthorizedHttpResult>> BlockUserAccount(HttpContext httpContext,
        IMessageBus bus, CancellationToken ct) =>
        SendForCurrentAccount(httpContext, bus, id => new BlockAccountCommand(id), ct);

    private static Task<Results<NoContent, UnauthorizedHttpResult>> UnblockUserAccount(HttpContext httpContext,
        IMessageBus bus, CancellationToken ct) =>
        SendForCurrentAccount(httpContext, bus, id => new UnblockAccountCommand(id), ct);

    private static Task<Results<NoContent, UnauthorizedHttpResult>> DeleteUserAccount(HttpContext httpContext,
        IMessageBus bus, CancellationToken ct) =>
        SendForCurrentAccount(httpContext, bus, id => new DeleteAccountCommand(id), ct);

    private static async Task<Results<NoContent, UnauthorizedHttpResult>> SendForCurrentAccount<TCommand>(
        HttpContext httpContext, IMessageBus bus, Func<Guid, TCommand> createCommand, CancellationToken ct)
        where TCommand : notnull
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return TypedResults.Unauthorized();

        await bus.InvokeAsync(createCommand(accountId), ct);
        return TypedResults.NoContent();
    }
}
