using Frelance.Application.Mediatr.Commands.Contracts;
using Frelance.Application.Mediatr.Queries.Contracts;
using Frelance.Contracts.Dtos;
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
        var createContractEndpoint = app.MapPost("/api/contracts",
                async (IMediator mediator, [FromForm] CreateContractRequest addContractRequest, CancellationToken ct) =>
                {
                    var command = addContractRequest.Adapt<CreateContractCommand>();
                    var result = await mediator.Send(command, ct);
                    return Results.Ok(result);
                })
            .Accepts<ContractsDto>("multipart/form-data")
            .WithTags("Contracts")
            .RequireAuthorization("ClientRole")
            .WithMetadata(new IgnoreAntiforgeryTokenAttribute());
        app.MapGet("/api/contracts/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var contract = await mediator.Send(new GetContractByIdQuery(id), ct);
                return Results.Ok(contract);
            }).WithTags("Contracts").
            RequireAuthorization();
        app.MapGet("/api/contracts", async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken ct) =>
            {
                var paginatedContracts = await mediator.Send(new GetContractsQuery
                    (new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
                return Results.Extensions.OkPaginationResult(paginatedContracts.PageSize, paginatedContracts.CurrentPage,
                    paginatedContracts.TotalCount, paginatedContracts.TotalPages, paginatedContracts.Items);
            }).WithTags("Contracts").
            RequireAuthorization();
        var updateContractEndpoint = app.MapPut("/api/contracts/{id}", async (IMediator mediator, int id,
                [FromForm] UpdateContractRequest updateContractRequest, CancellationToken ct) =>
            {
                var command = updateContractRequest.Adapt<UpdateContractCommand>() with { Id = id };
                var result = await mediator.Send(command, ct);
                return Results.Ok(result);
            }).WithTags("Contracts").
            RequireAuthorization();
        app.MapDelete("/api/contracts/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var command = new DeleteContractCommand(id);
                var result = await mediator.Send(command, ct);
                return Results.Ok(result);
            }).WithTags("Contracts").
            RequireAuthorization();
        createContractEndpoint.RemoveAntiforgery();
        updateContractEndpoint.RemoveAntiforgery();
    }

}