using System.Security.Claims;
using Freelance.Identity.Application.Mediatr.Accounts.Commands;
using Freelance.Identity.Application.Mediatr.Accounts.Query;
using Shared.Application.Mediator;

namespace Freelance.Identity.Api.Modules;

public static class UserModules
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup("/api/auth")
            .WithTags("Users");
        var authenticatedGroup = app.MapGroup("/api/auth")
            .WithTags("Users")
            .RequireAuthorization();

        userGroup.MapPost("/register", async (IMediator mediator, CreateAccountCommand createAccountCommand,
            CancellationToken ct) =>
        {
            var createdAccount = await mediator.Send(createAccountCommand, ct);
            return Results.Created($"/api/auth/{createdAccount.Id}", createdAccount);
        });

        userGroup.MapPost("/login", async (IMediator mediator, LoginQuery loginQuery,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(loginQuery, ct);
            return Results.Ok(result);
        });

        authenticatedGroup.MapPut("/block/{id:guid}",
            async (IMediator mediator, Guid id, CancellationToken ct) =>
            {
                var blockAccountCommand = new BlockAccountCommand
                {
                    AccountId = id
                };

                await mediator.Send(blockAccountCommand, ct);
                return Results.NoContent();
            });

        authenticatedGroup.MapGet("/current",
            async (IMediator mediator, HttpContext httpContext, CancellationToken ct) =>
            {
                var username = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username)) return Results.Unauthorized();

                var currentUserQuery = new GetCurrentAccountQuery
                {
                    Username = username
                };
                var result = await mediator.Send(currentUserQuery, ct);
                return Results.Ok(result);
            });

        authenticatedGroup.MapPut("/unblock/{id:guid}", async (IMediator mediator, Guid id, CancellationToken ct) =>
        {
            var unblockAccountCommand = new UnblockAccountCommand
            {
                AccountId = id
            };

            await mediator.Send(unblockAccountCommand, ct);
            return Results.NoContent();
        });

        authenticatedGroup.MapDelete("/{id:guid}", async (IMediator mediator, Guid id, CancellationToken ct) =>
        {
            var deleteAccountCommand = new DeleteAccountCommand
            {
                Id = id
            };

            await mediator.Send(deleteAccountCommand, ct);
            return Results.NoContent();
        });
    }
}