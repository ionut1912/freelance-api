using Frelance.Contracts.Dtos;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.Invoices;

public record GetInvoiceByIdQuery(int Id) : IRequest<InvoicesDto>;
