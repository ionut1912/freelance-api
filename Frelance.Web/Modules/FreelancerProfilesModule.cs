using Frelance.Application.Mediatr.Commands.UserProfile;
using Frelance.Contracts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class FreelancerProfilesModule
{
    public static void AddFreelancerProfilesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPatch("/api/freelancerProfiles/freelancerDetails/{id:int}", async (IMediator mediator, int id,
                [FromBody] FreelancerProfileData freelancerProfileData, CancellationToken ct) =>
            {
                await mediator.Send(new PatchFreelancerDataCommand(id, freelancerProfileData), ct);
                return Results.NoContent();
            }).WithTags("FreelancerProfiles")
            .RequireAuthorization("FreelancerRole");
    }
}