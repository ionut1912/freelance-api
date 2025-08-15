using Frelance.Application.Mediatr.Commands.Invoices;
using Frelance.Application.Mediatr.Queries.Invoices;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Requests.Invoices;
using Frelance.Web.Extensions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Frelance.Web.Modules;

public static class InvoicesModule
{
    public static void AddInvoicesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/invoice",
                async (IMediator mediator, CreateInvoiceRequest createInvoiceRequest,
                    CancellationToken ct) =>
                {
                    await mediator.Send(createInvoiceRequest.Adapt<CreateInvoiceCommand>(), ct);
                    return Results.Created();
                })
            .WithTags("Invoices")
            .RequireAuthorization("FreelancerRole");

        app.MapGet("/api/invoices/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var invoice = await mediator.Send(new GetInvoiceByIdQuery(id), ct);
                return Results.Ok(invoice);
            }).WithTags("Invoices")
            .RequireAuthorization();

        app.MapGet("/api/invoices",
            async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken ct) =>
            {
                var paginatedInvoices = await mediator.Send(new GetInvoicesQuery
                    (new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
                return Results.Extensions.OkPaginationResult(paginatedInvoices.PageSize, paginatedInvoices.CurrentPage,
                    paginatedInvoices.TotalCount, paginatedInvoices.TotalPages, paginatedInvoices.Items);
            }).WithTags("Invoices").RequireAuthorization();

        app.MapPut("/api/invoices/{id}", async (IMediator mediator, int id,
                UpdateInvoiceRequest updateInvoiceRequest, CancellationToken ct) =>
            {
                var command = updateInvoiceRequest.Adapt<UpdateInvoiceCommand>() with { Id = id };
                await mediator.Send(command, ct);
                return Results.NoContent();
            }).WithTags("Invoices")
            .RequireAuthorization();
        app.MapDelete("/api/invoices/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
        {
            var command = new DeleteInvoiceCommand(id);
            await mediator.Send(command, ct);
            return Results.NoContent();
        }).WithTags("Invoices").RequireAuthorization();
    }
}