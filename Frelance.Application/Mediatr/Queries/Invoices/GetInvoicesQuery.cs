using Frelance.Contracts.Dtos;
using Frelance.Contracts.Requests.Common;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Queries.Invoices;

public record GetInvoicesQuery(PaginationParams PaginationParams):IRequest<PaginatedList<InvoicesDto>>;