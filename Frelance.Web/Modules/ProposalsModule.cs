using Frelance.Application.Mediatr.Commands.Proposals;
using Frelance.Application.Mediatr.Queries.Projects;
using Frelance.Application.Mediatr.Queries.Proposals;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Requests.Proposals;
using Frelance.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class ProposalsModule
{
    public static void AddProposalEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/proposals", async (IMediator mediator, CreateProposalRequest CreateProposalRequest,
                CancellationToken ct) =>
            {
                var command = new CreateProposalCommand(CreateProposalRequest);
                var result = await mediator.Send(command, ct);
                return Results.Ok(result);
            }).WithTags("Proposals")
            .RequireAuthorization();
        app.MapGet("/api/proposals/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var proposal = await mediator.Send(new GetProposalByIdQuery(id), ct);
                return Results.Ok(proposal);
            }).WithTags("Proposals").
            RequireAuthorization();
        app.MapGet("/api/proposals", async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken ct) =>
            {
                var paginatedProposals = await mediator.Send(new GetProposalsQuery
                    (new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
                return Results.Extensions.OkPaginationResult(paginatedProposals.PageSize, paginatedProposals.CurrentPage,
                    paginatedProposals.TotalCount, paginatedProposals.TotalPages, paginatedProposals.Items);
            }).WithTags("Proposals")
            .RequireAuthorization();

        app.MapPut("/api/proposals/{id}", async (IMediator mediator, int id,
                UpdateProposalRequest updateProposalRequest, CancellationToken ct) =>
            {
                var command = new UpdateProposalCommand(id, updateProposalRequest);
                var result = await mediator.Send(command, ct);
                return Results.Ok(result);
            }).WithTags("Proposals")
            .RequireAuthorization();
        app.MapDelete("/api/proposals/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var command = new DeleteProposalCommand(id);
                var result = await mediator.Send(command, ct);
                return Results.Ok(result);
            }).WithTags("Proposals")
            .RequireAuthorization();
    }
}