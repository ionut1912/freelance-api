using Frelance.Application.Mediatr.Commands.Users;
using Frelance.Contracts.Dtos;
using Mapster;
using MediatR;

namespace Frelance.Web.Modules;

public static class UserModule
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/register", async (IMediator mediator, RegisterDto registerDto,
            CancellationToken ct) =>
        {
            await mediator.Send(registerDto.Adapt<CreateUserCommand>(), ct);
            return Results.Created();
        }).WithTags("Users");
        app.MapPost("/api/login", async (IMediator mediator, LoginDto loginDto,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(loginDto.Adapt<LoginCommand>(), ct);
            return Results.Ok(result);
        }).WithTags("Users");
    }
}