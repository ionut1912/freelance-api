using Freelance.Contracts.Dtos;
using Freelance.Contracts.Requests.Common;
using Freelance.Contracts.Responses.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Queries.Invoices;

public record GetInvoicesQuery(PaginationParams PaginationParams) : IRequest<PaginatedList<InvoicesDto>>;