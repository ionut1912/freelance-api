using Freelance.Application.Mediatr.Queries.Invoices;
using Freelance.Application.Repositories;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Responses.Common;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Invoices;

public class GetInvoicesQueryHandler : IRequestHandler<GetInvoicesQuery, PaginatedList<InvoicesDto>>
{
    private readonly IInvoiceRepository _invoiceRepository;

    public GetInvoicesQueryHandler(IInvoiceRepository invoiceRepository)
    {
        ArgumentNullException.ThrowIfNull(invoiceRepository, nameof(invoiceRepository));
        _invoiceRepository = invoiceRepository;
    }

    public async Task<PaginatedList<InvoicesDto>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        return await _invoiceRepository.GetInvoicesAsync(request, cancellationToken);
    }
}