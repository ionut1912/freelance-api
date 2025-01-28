using Frelance.Application.Mediatr.Commands.Users;
using Frelance.Contracts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Frelance.Web.Modules;

public static class UserModule
{
    public static void AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users", async (IMediator mediator, RegisterDto registerDto,
            CancellationToken ct) =>
        {
            var command = new CreateUserCommand(registerDto, new ModelStateDictionary());
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithTags("Users");
    }
}