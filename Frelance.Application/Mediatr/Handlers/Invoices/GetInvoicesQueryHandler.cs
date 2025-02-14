using Frelance.Application.Mediatr.Queries.Invoices;
using Frelance.Application.Repositories;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Invoices;

public class GetInvoicesQueryHandler:IRequestHandler<GetInvoicesQuery,PaginatedList<InvoicesDto>>
{
    private readonly IInvoiceRepository _invoiceRepository;

    public GetInvoicesQueryHandler(IInvoiceRepository invoiceRepository)
    {
        ArgumentNullException.ThrowIfNull(invoiceRepository, nameof(invoiceRepository));
        _invoiceRepository = invoiceRepository;
    }
    
    public  async Task<PaginatedList<InvoicesDto>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        return await _invoiceRepository.GetInvoicesAsync(request, cancellationToken);
    }
}