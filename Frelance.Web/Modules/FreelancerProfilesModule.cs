using Frelance.Application.Mediatr.Commands.UserProfile;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Enums;
using Frelance.Contracts.Requests.FreelancerProfiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class FreelancerProfilesModule
{
    public static void AddFreelancerProfilesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/freelancerProfiles",
                async (IMediator mediator, CreateFreelancerProfileRequest createFreelancerProfileRequest,
                    CancellationToken ct) =>
                {
                    await mediator.Send(new CreateUserProfileCommand(Role.Freelancer, createFreelancerProfileRequest),
                        ct);
                    return Results.Created();
                })
            .WithTags("FreelancerProfiles")
            .RequireAuthorization("FreelancerRole");
        
        app.MapPatch("/api/freelancerProfiles/freelancerDetails/{id:int}", async (IMediator mediator, int id,
                [FromBody] FreelancerProfileData freelancerProfileData, CancellationToken ct) =>
            {
                await mediator.Send(new PatchFreelancerDataCommand(id, freelancerProfileData), ct);
                return Results.NoContent();
            }).WithTags("FreelancerProfiles")
            .RequireAuthorization("FreelancerRole");
    }
}