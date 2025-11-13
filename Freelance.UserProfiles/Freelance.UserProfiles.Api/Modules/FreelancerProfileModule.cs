using System.Runtime.InteropServices.ComTypes;
using Freelance.UserProfiles.Api.Modules.Extensions;
using Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;
using Freelancer.UserProfiles.Application.Mediatr.FreelancerProfiles.Queies;
using Freelancer.UserProfiles.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Freelance.UserProfiles.Api.Modules;

public static class FreelancerProfileModule
{
    public static void AddFreelancerProfileEndpoints(this IEndpointRouteBuilder app)
    {
        var clientOnlyGroup = app.MapGroup("/api/freelancer-profiles")
            .WithTags("FreelancerProfile")
            .RequireAuthorization("ClientOnly");
        var freelancerOnlyGroup = app.MapGroup("/api/freelancer-profiles")
            .WithTags("FreelancerProfile")
            .RequireAuthorization("FreelancerOnly");
        var authenticatedGroup = app.MapGroup("/api/freelancer-profiles")
            .WithTags("FreelancerProfile")
            .RequireAuthorization();

        freelancerOnlyGroup.MapPost("/", async (IMediator mediator,HttpContext httpContext,CreateFreelancerProfileRequest createFreelancerProfileRequest,CancellationToken ct) =>
        {
            var accountId = httpContext.GetAccountId();
            if (accountId == Guid.Empty)
            {
                return Results.Unauthorized();
            }

            var createFreelancerProfileCommand = new CreateFreelancerProfileCommand
            {
                AccountId = accountId,
                Address = createFreelancerProfileRequest.Address,
                Bio = createFreelancerProfileRequest.Bio,
                Image = createFreelancerProfileRequest.Image,
                Experience = createFreelancerProfileRequest.Experience,
                Amount = createFreelancerProfileRequest.Amount,
                Currency = createFreelancerProfileRequest.Currency,
                PortfolioUrl = createFreelancerProfileRequest.PortfolioUrl,
                ForeignLanguages = createFreelancerProfileRequest.ForeignLanguages,
                ProgrammingLanguages = createFreelancerProfileRequest.ProgrammingLanguages,
                Areas = createFreelancerProfileRequest.Areas
            };
            var createdFreelancerProfile = await mediator.Send(createFreelancerProfileCommand, ct);
            return Results.Created($"/api/freelancer-profiles/{createdFreelancerProfile.Id}", createdFreelancerProfile);
        });

        authenticatedGroup.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var freelancers = await mediator.Send(new GetFreelancerProfilesQuery(), ct);
            return Results.Ok(freelancers);
        });

        freelancerOnlyGroup.MapGet("/current",
            async (IMediator mediator, HttpContext httpContext, CancellationToken ct) =>
            {
                var accountId = httpContext.GetAccountId();
                if (accountId == Guid.Empty)
                {
                    return Results.Unauthorized();
                }

                var getCurrentFreelancerQuery = new GetLoggedInFreelancerProfileQuery
                {
                    AccountId = accountId
                };

                var currentFreelancer = await mediator.Send(getCurrentFreelancerQuery, ct);
                return Results.Ok(currentFreelancer);
            });

        freelancerOnlyGroup.MapPut("/details/{id:guid}", async (IMediator mediator,Guid id,[FromBody] UpdateFreelancerDetailRequest updateFreelancerDetailRequest,CancellationToken ct) =>
        {
            var updateFreelancerDetailsCommand = new UpdateFreelancerDetailsCommand
            {
                Id = id,
                ForeignLanguages = updateFreelancerDetailRequest.ForeignLanguages,
                ProgrammingLanguages = updateFreelancerDetailRequest.ProgrammingLanguages,
                Areas = updateFreelancerDetailRequest.Areas,
                Experience = updateFreelancerDetailRequest.Experience,
                Amount = updateFreelancerDetailRequest.Amount,
                Currency = updateFreelancerDetailRequest.Currency,
                PortfolioUrl = updateFreelancerDetailRequest.PortfolioUrl
            };
            await mediator.Send(updateFreelancerDetailsCommand, ct);
            return Results.NoContent();
        });

        freelancerOnlyGroup.MapPut("/address/{id:guid}", async (IMediator mediator,Guid id,[FromBody] UpdateProfileAddressRequest updateProfileAddressRequest,CancellationToken ct) =>
        {
            var updateFreelancerAddressCommand = new UpdateFreelancerProfileAddressCommand
            {
                Id = id,
                AddressDto = updateProfileAddressRequest.AddressDto
            };
            await mediator.Send(updateFreelancerAddressCommand, ct);
            return Results.NoContent();
        });

        freelancerOnlyGroup.MapPut("/data/{id:guid}",
            async (IMediator mediator,Guid id, [FromBody] UpdateProfileDataRequest updateProfileDataRequest, CancellationToken ct) =>
            {
                var updateFreelancerProfileDataCommand = new UpdateFreelancerProfileDataCommand
                {
                    Id = id,
                    Bio = updateProfileDataRequest.Bio,
                    Image = updateProfileDataRequest.Image
                };
                await mediator.Send(updateFreelancerProfileDataCommand, ct);
                return Results.NoContent();
            });
        
        clientOnlyGroup.MapPut("/rating/{id:guid}", async (IMediator mediator,Guid id,[FromBody] UpdateFreelancerProfileRatingRequest updateFreelancerProfileRatingRequest,CancellationToken ct) =>
        {
            var updateFreelancerRatingCommand = new UpdateFreelancerRatingCommand
            {
                Id = id,
                Rating = updateFreelancerProfileRatingRequest.Rating
            };
            await mediator.Send(updateFreelancerRatingCommand, ct);
            return Results.NoContent();
        });

        freelancerOnlyGroup.MapDelete("/{id:guid}", async (IMediator mediator, Guid id, CancellationToken ct) =>
        {
            var deleteFreelancerProfileCommand = new DeleteFreelancerProfileCommand
            {
                Id = id
            };
            await mediator.Send(deleteFreelancerProfileCommand, ct);
            return Results.NoContent();
        });
    }
    
}