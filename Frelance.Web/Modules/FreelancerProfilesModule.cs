using Frelance.Application.Mediatr.Commands.ClientProfiles;
using Frelance.Application.Mediatr.Commands.FreelancerProfiles;
using Frelance.Application.Mediatr.Queries.FreelancerProfiles;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Address;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Requests.FreelancerProfiles;
using Frelance.Contracts.Requests.Skills;
using Frelance.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class FreelancerProfilesModule
{
    public static void AddFreelancerProfilesEndpoints(this IEndpointRouteBuilder app)
    {
        var createFreelancerProfileEndpoint = app.MapPost("/api/freelancerProfiles",
                async (IMediator mediator,  [FromForm]CreateFreelancerProfileRequest createFreelancerProfileRequest, CancellationToken ct) =>
                {
                    var address = new AddressRequest(
                        createFreelancerProfileRequest.AddressCountry,
                        createFreelancerProfileRequest.AddressCity,
                        createFreelancerProfileRequest.AddressStreet,
                        createFreelancerProfileRequest.AddressStreetNumber,
                        createFreelancerProfileRequest.AddressZip);
                    var skills=new List<SkillRequest>();
                    if (createFreelancerProfileRequest is { ProgrammingLanguages: not null, Areas: not null })
                    {
                        var count = Math.Min(createFreelancerProfileRequest.ProgrammingLanguages.Count, createFreelancerProfileRequest.Areas.Count);
                        for (var i = 0; i < count; i++)
                        {
                            skills.Add(new SkillRequest(createFreelancerProfileRequest.ProgrammingLanguages[i], createFreelancerProfileRequest.Areas[i]));
                        }
                    }
                    var command = new AddFreelancerProfileCommand(address, createFreelancerProfileRequest.Bio, createFreelancerProfileRequest.ProfileImage,skills,createFreelancerProfileRequest.ForeignLanguages,createFreelancerProfileRequest.Experience,createFreelancerProfileRequest.Rate,createFreelancerProfileRequest.Currency,createFreelancerProfileRequest.Rating,createFreelancerProfileRequest.PortfolioUrl);
                    var result = await mediator.Send(command, ct);
                    return Results.Ok(result);
                })
            .Accepts<FreelancerProfileDto>("multipart/form-data")
            .WithTags("FreelancerProfiles")
            .RequireAuthorization("FreelancerRole")
            .WithMetadata(new IgnoreAntiforgeryTokenAttribute());
        app.MapGet("/api/freelancerProfiles/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var freelancerProfile = await mediator.Send(new GetFreelancerProfileByIdQuery(id), ct);
                return Results.Ok(freelancerProfile);
            }).WithTags("FreelancerProfiles").
            RequireAuthorization("FreelancerRole");
        app.MapGet("/api/freelancerProfiles", async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken ct) =>
            {
                var paginatedFreelancerProfiles = await mediator.Send(new GetFreelancerProfilesQuery
                    (new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
                return Results.Extensions.OkPaginationResult(paginatedFreelancerProfiles.PageSize, paginatedFreelancerProfiles.CurrentPage,
                    paginatedFreelancerProfiles.TotalCount, paginatedFreelancerProfiles.TotalPages, paginatedFreelancerProfiles.Items);
            }).WithTags("FreelancerProfiles").
            RequireAuthorization("FreelancerRole");
        var updateFreelancerProfileEndpoint = app.MapPut("/api/freelancerProfiles/{id}", async (IMediator mediator, int id,
                [FromForm] UpdateFreelancerProfileRequest updateFreelancerProfileRequest, CancellationToken ct) =>
            {
                var address = new AddressRequest(
                    updateFreelancerProfileRequest.AddressCountry,
                    updateFreelancerProfileRequest.AddressCity,
                    updateFreelancerProfileRequest.AddressStreet,
                    updateFreelancerProfileRequest.AddressStreetNumber,
                    updateFreelancerProfileRequest.AddressZip);
                var skills=new List<SkillRequest>();
                if (updateFreelancerProfileRequest is { ProgrammingLanguages: not null, Areas: not null })
                {
                    var count = Math.Min(updateFreelancerProfileRequest.ProgrammingLanguages.Count, updateFreelancerProfileRequest.Areas.Count);
                    for (var i = 0; i < count; i++)
                    {
                        skills.Add(new SkillRequest(updateFreelancerProfileRequest.ProgrammingLanguages[i], updateFreelancerProfileRequest.Areas[i]));
                    }
                }
                var command = new UpdateFreelancerProfileCommand(id, address, updateFreelancerProfileRequest.Bio, updateFreelancerProfileRequest.ProfileImage,skills,updateFreelancerProfileRequest.ForeignLanguages,updateFreelancerProfileRequest.Experience,updateFreelancerProfileRequest.Rate,updateFreelancerProfileRequest.Currency,updateFreelancerProfileRequest.Rating,updateFreelancerProfileRequest.PortfolioUrl);
                var result = await mediator.Send(command, ct);
                return Results.Ok(result);
            }).WithTags("FreelancerProfiles").
            RequireAuthorization("FreelancerRole");
        app.MapDelete("/api/freelancerProfiles/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var command = new DeleteFreelancerProfileCommand(id);
                var result = await mediator.Send(command, ct);
                return Results.Ok(result);
            }).WithTags("FreelancerProfiles").
            RequireAuthorization("FreelancerRole");
        createFreelancerProfileEndpoint.RemoveAntiforgery();
        updateFreelancerProfileEndpoint.RemoveAntiforgery();
    }
}