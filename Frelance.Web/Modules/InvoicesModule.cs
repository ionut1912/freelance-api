using Frelance.Application.Mediatr.Commands.FreelancerProfiles;
using Frelance.Application.Mediatr.Commands.Invoices;
using Frelance.Application.Mediatr.Queries.Invoices;
using Frelance.Contracts.Dtos;
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
        var createInvoiceEnpoint = app.MapPost("/api/invoice",
                async (IMediator mediator, [FromForm] CreateInvoiceRequest createInvoiceRequest,
                    CancellationToken ct) =>
                {
                    var result = await mediator.Send(createInvoiceRequest.Adapt<CreateInvoiceCommand>(), ct);
                    return Results.Ok(result);
                })
            .Accepts<InvoicesDto>("multipart/form-data")
            .WithTags("Invoices")
            .RequireAuthorization("FreelancerRole")
            .WithMetadata(new IgnoreAntiforgeryTokenAttribute());
        app.MapGet("/api/invoices/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
                    {
                        var invoice = await mediator.Send(new GetInvoiceByIdQuery(id), ct);
                        return Results.Ok(invoice);
                    }).WithTags("Invoices").
                    RequireAuthorization();
        app.MapGet("/api/invoices", async (IMediator mediator, [FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken ct) =>
            {
                var paginatedInvoices = await mediator.Send(new GetInvoicesQuery
                    (new PaginationParams { PageSize = pageSize, PageNumber = pageNumber }), ct);
                return Results.Extensions.OkPaginationResult(paginatedInvoices.PageSize, paginatedInvoices.CurrentPage,
                    paginatedInvoices.TotalCount, paginatedInvoices.TotalPages, paginatedInvoices.Items);
            }).WithTags("Invoices").
            RequireAuthorization();
        var updateInvoiceEndpoint = app.MapPut("/api/invoices/{id}", async (IMediator mediator, int id,
            [FromForm] UpdateInvoiceRequest updateInvoiceRequest, CancellationToken ct) =>
        {
            var command = updateInvoiceRequest.Adapt<UpdateInvoiceCommand>() with { Id = id };
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithTags("Invoices")
            .RequireAuthorization();
        app.MapDelete("/api/invoices/{id}", async (IMediator mediator, int id, CancellationToken ct) =>
            {
                var command = new DeleteInvoiceCommand(id);
                var result = await mediator.Send(command, ct);
                return Results.Ok(result);
            }).WithTags("Invoices").
            RequireAuthorization();
        createInvoiceEnpoint.RemoveAntiforgery();
        updateInvoiceEndpoint.RemoveAntiforgery();
    }
}