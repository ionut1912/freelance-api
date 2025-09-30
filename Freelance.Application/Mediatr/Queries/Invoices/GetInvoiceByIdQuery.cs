using Freelance.Contracts.Dtos;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.Invoices;

public record GetInvoiceByIdQuery(int Id) : IRequest<InvoicesDto>;