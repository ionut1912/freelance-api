using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Mediatr.Accounts.Query;
using Shared.Api.Extensions;
using Shared.Application.Mediator;
using System.Security.Claims;

namespace Freelance.Identity.Api.Endpoints.Modules;

public static class UserModules
{
    public static IEndpointRouteBuilder AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup("/api/auth")
            .WithTags("Users");
        var authenticatedGroup = app.MapGroup("/api/auth")
            .WithTags("Users")
            .RequireAuthorization();

        userGroup.MapPost("/register", RegisterUser);
        userGroup.MapPost("/login", LoginUser);
        authenticatedGroup.MapPut("/block", BlockUserAccount);
        authenticatedGroup.MapGet("/current", GetCurrentUser);
        authenticatedGroup.MapPut("/unblock", UnblockUserAccount);
        authenticatedGroup.MapDelete("/{id:guid}", DeleteUserAccount);

        return app;
    }

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

    private static async Task<IResult> BlockUserAccount(IMediator mediator, HttpContext httpContext, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return Results.Unauthorized();

        var blockAccountCommand = new BlockAccountCommand(accountId);
        await mediator.Send(blockAccountCommand, ct);
        return Results.NoContent();
    }

    private static async Task<IResult> GetCurrentUser(IMediator mediator, HttpContext httpContext, CancellationToken ct)
    {
        var username = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrEmpty(username)) return Results.Unauthorized();

        var currentUserQuery = new GetCurrentAccountQuery(username);
        var result = await mediator.Send(currentUserQuery, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> UnblockUserAccount(IMediator mediator, HttpContext httpContext, CancellationToken ct)
    {
        var accountId = httpContext.GetAccountId();
        if (accountId == Guid.Empty) return Results.Unauthorized();

        var unblockAccountCommand = new UnblockAccountCommand(accountId);
        await mediator.Send(unblockAccountCommand, ct);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteUserAccount(IMediator mediator, Guid id, CancellationToken ct)
    {
        var deleteAccountCommand = new DeleteAccountCommand(id);
        await mediator.Send(deleteAccountCommand, ct);
        return Results.NoContent();
    }
}