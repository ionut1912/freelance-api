using Frelance.Application.Mediatr.Commands.Users;
using Frelance.Contracts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Frelance.Web.Modules;

public static class UserModule
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/register", async (IMediator mediator, RegisterDto registerDto,
            CancellationToken ct) =>
        {
            var command = new CreateUserCommand(registerDto, new ModelStateDictionary());
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithTags("Users");
        app.MapPost("/api/login", async (IMediator mediator, LoginDto loginDto,
            CancellationToken ct) =>
        {
            var command = new LoginCommand(loginDto);
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithTags("Users");
        
    }
}