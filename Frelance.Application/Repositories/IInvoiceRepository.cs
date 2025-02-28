using Frelance.Application.Mediatr.Commands.Invoices;
using Frelance.Application.Mediatr.Queries.Invoices;
using Frelance.Contracts.Dtos;
using Frelance.Contracts.Responses.Common;

namespace Frelance.Application.Repositories;

public interface IInvoiceRepository
{
    Task CreateInvoiceAsync(CreateInvoiceCommand createInvoiceCommand, CancellationToken cancellationToken);
    Task<InvoicesDto> GetInvoiceByIdAsync(GetInvoiceByIdQuery query, CancellationToken cancellationToken);
    Task<PaginatedList<InvoicesDto>> GetInvoicesAsync(GetInvoicesQuery query, CancellationToken cancellationToken);
    Task UpdateInvoiceAsync(UpdateInvoiceCommand updateInvoiceCommand, CancellationToken cancellationToken);
    Task DeleteInvoiceAsync(DeleteInvoiceCommand deleteInvoiceCommand, CancellationToken cancellationToken);
}