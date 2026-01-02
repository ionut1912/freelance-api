using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Mediatr.Accounts.Query;
using Microsoft.AspNetCore.Authorization;
using Shared.Api.Extensions;
using Shared.Api.Infrastructure;
using Shared.Application.Mediator;
using System.Security.Claims;

namespace Freelance.Identity.Api.Endpoints;

public class UserEndpointsGroup : EndpointGroup
{
    public override void Map(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(this);

        group.MapPost(RegisterUser, "/register");
        group.MapPost(LoginUser, "/login");
        group.MapPut(BlockUserAccount, "/block");
        group.MapGet(GetCurrentUser, "/current");
        group.MapPut(UnblockUserAccount, "/unblock");
        group.MapDelete(DeleteUserAccount, "/");
    }+

    private static async Task<IResult> RegisterUser(IMediator mediator, CreateAccountCommand createAccountCommand,
        CancellationToken ct)
    {
        var createdAccount = await mediator.Send(createAccountCommand, ct);
        return Results.Created($"/api/auth/{createdAccount.Id}", createdAccount);
    }

    private static async Task<IResult> LoginUser(IMediator mediator, LoginQuery loginQuery,
        CancellationToken ct)
    {
        var result = await mediator.Send(loginQuery, ct);
        return Results.Ok(result);
    }

    [Authorize]
    private static async Task<IResult> BlockUserAccount(IMediator mediator, HttpContext httpContext, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return Results.Unauthorized();

        var blockAccountCommand = new BlockAccountCommand(accountId);
        await mediator.Send(blockAccountCommand, ct);
        return Results.NoContent();
    }

    [Authorize]
    private static async Task<IResult> GetCurrentUser(IMediator mediator, HttpContext httpContext, CancellationToken ct)
    {
        var username = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(username)) return Results.Unauthorized();

        var currentUserQuery = new GetCurrentAccountQuery(username);
        var result = await mediator.Send(currentUserQuery, ct);
        return Results.Ok(result);
    }

    [Authorize]
    private static async Task<IResult> UnblockUserAccount(IMediator mediator, HttpContext httpContext, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return Results.Unauthorized();

        var unblockAccountCommand = new UnblockAccountCommand(accountId);
        await mediator.Send(unblockAccountCommand, ct);
        return Results.NoContent();
    }

    [Authorize]
    private static async Task<IResult> DeleteUserAccount(IMediator mediator, HttpContext httpContext, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return Results.Unauthorized();

        var deleteAccountCommand = new DeleteAccountCommand(accountId);
        await mediator.Send(deleteAccountCommand, ct);
        return Results.NoContent();
    }
}