using Frelance.Application.Mediatr.Skills;
using MediatR;

namespace Frelance.Web.Modules;

public static class SkillsModule
{
    public static void AddSkillsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/skills",
                async (IMediator mediator) =>
                {
                    var skills = await mediator.Send(new GetSkillsQuery());
                    return Results.Ok(skills);
                }).WithTags("Skills")
            .RequireAuthorization();
    }
}