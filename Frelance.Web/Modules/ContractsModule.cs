using Frelance.Application.Mediatr.Commands.Contracts;
using Frelance.Application.Mediatr.Queries.Contracts;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Requests.Contracts;
using Frelance.Web.Extensions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class ContractsModule
{
    public static void AddContractEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/contracts",
                async (IMediator mediator, CreateContractRequest addContractRequest, CancellationToken ct) =>
                {
                    var command = addContractRequest.Adapt<CreateContractCommand>();
                    await mediator.Send(command, ct);
                    return Results.Created();
                })
            .WithTags("Contracts")
            .RequireAuthorization("ClientRole");
        app.MapGet("/api/contracts/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
        {
            var contract = await mediator.Send(new GetContractByIdQuery(id), ct);
            return Results.Ok(contract);
        }).WithTags("Contracts").RequireAuthorization();
        app.MapGet("/api/contracts",
            async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken ct) =>
            {
                var paginatedContracts = await mediator.Send(new GetContractsQuery
                    (new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
                return Results.Extensions.OkPaginationResult(paginatedContracts.PageSize,
                    paginatedContracts.CurrentPage,
                    paginatedContracts.TotalCount, paginatedContracts.TotalPages, paginatedContracts.Items);
            }).WithTags("Contracts").RequireAuthorization();
        app.MapPut("/api/contracts/{id}", async (IMediator mediator, int id,
            UpdateContractRequest updateContractRequest, CancellationToken ct) =>
        {
            var command = updateContractRequest.Adapt<UpdateContractCommand>() with { Id = id };
            await mediator.Send(command, ct);
            return Results.NoContent();
        }).WithTags("Contracts").RequireAuthorization();
        app.MapDelete("/api/contracts/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
        {
            var command = new DeleteContractCommand(id);
            await mediator.Send(command, ct);
            return Results.NoContent();
        }).WithTags("Contracts").RequireAuthorization();
    }
}