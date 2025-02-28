using System.Security.Claims;
using Frelance.Application.Mediatr.Commands.Face;
using Frelance.Contracts.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class FaceModule
{
    public static void AddFaceEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/verifyFace", async (IMediator mediator, [FromBody] string faceImageBase64, HttpContext httpContext, CancellationToken cancellationToken) =>
            {
                var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                var commandRole = role == "Client" ? Role.Client : Role.Freelancer;
                var command = new VerifyFaceCommand(commandRole, faceImageBase64);
                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);

            })
            .WithTags("FaceRecognition")
            .RequireAuthorization();
    }

}