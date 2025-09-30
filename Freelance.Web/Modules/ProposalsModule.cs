using Freelance.Application.Mediatr.Commands.Proposals;
using Freelance.Application.Mediatr.Queries.Proposals;
using Freelance.Contracts.Requests.Common;
using Freelance.Contracts.Requests.Proposals;
using Freelance.Web.Extensions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Freelance.Web.Modules;

public static class ProposalsModule
{
    public static void AddProposalEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/proposals", async (IMediator mediator, CreateProposalRequest createProposalRequest,
                CancellationToken ct) =>
            {
                await mediator.Send(createProposalRequest.Adapt<CreateProposalCommand>(), ct);
                return Results.Created();
            }).WithTags("Proposals")
            .RequireAuthorization();
        app.MapGet("/api/proposals/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
        {
            var proposal = await mediator.Send(new GetProposalByIdQuery(id), ct);
            return Results.Ok(proposal);
        }).WithTags("Proposals").RequireAuthorization();
        app.MapGet("/api/proposals",
                async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber,
                    CancellationToken ct) =>
                {
                    var paginatedProposals = await mediator.Send(new GetProposalsQuery
                        (new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
                    return Results.Extensions.OkPaginationResult(paginatedProposals.PageSize,
                        paginatedProposals.CurrentPage,
                        paginatedProposals.TotalCount, paginatedProposals.TotalPages, paginatedProposals.Items);
                }).WithTags("Proposals")
            .RequireAuthorization();

        app.MapPut("/api/proposals/{id:int}", async (IMediator mediator, int id,
                UpdateProposalRequest updateProposalRequest, CancellationToken ct) =>
            {
                var command = updateProposalRequest.Adapt<UpdateProposalCommand>() with { Id = id };
                await mediator.Send(command, ct);
                return Results.NoContent();
            }).WithTags("Proposals")
            .RequireAuthorization();
        app.MapDelete("/api/proposals/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var command = new DeleteProposalCommand(id);
                await mediator.Send(command, ct);
                return Results.NoContent();
            }).WithTags("Proposals")
            .RequireAuthorization();
    }
}