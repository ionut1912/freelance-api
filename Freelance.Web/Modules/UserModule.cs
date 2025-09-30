using Freelance.Application.Mediatr.Commands.Users;
using Freelance.Contracts.Dtos;
using Freelance.Web.Modules.Utils;
using Mapster;
using MediatR;

namespace Freelance.Web.Modules;

public static class UserModule
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register", async (IMediator mediator, RegisterDto registerDto,
            CancellationToken ct) =>
        {
            await mediator.Send(registerDto.Adapt<CreateUserCommand>(), ct);
            return Results.Created();
        }).WithTags("Users");
        app.MapPost("/api/auth/login", async (IMediator mediator, LoginDto loginDto,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(loginDto.Adapt<LoginCommand>(), ct);
            return Results.Ok(result);
        }).WithTags("Users");

        app.MapPost("/api/auth/block/{id:int}",
                async (IMediator mediator, int id, CancellationToken ct) =>
                {
                    await mediator.Send(new BlockAccountCommand(id.ToString()), ct);
                }).WithTags("Users")
            .RequireAuthorization();
        
        app.MapDelete("/api/auth/account/current",
                async (IMediator mediator, HttpContext httpContext,CancellationToken ct) =>
                {
                    await mediator.Send(new DeleteCurrentAccountCommand(httpContext.GetRole()), ct);
                }).WithTags("Users")
            .RequireAuthorization();
    }
}