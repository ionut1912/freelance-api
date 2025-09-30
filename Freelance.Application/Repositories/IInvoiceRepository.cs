using Freelance.Application.Mediatr.Commands.Invoices;
using Freelance.Application.Mediatr.Queries.Invoices;
using Freelance.Contracts.Dtos;
using Freelance.Contracts.Responses.Common;

namespace Freelance.Application.Repositories;

public interface IInvoiceRepository
{
    Task CreateInvoiceAsync(CreateInvoiceCommand createInvoiceCommand, CancellationToken cancellationToken);
    Task<InvoicesDto> GetInvoiceByIdAsync(GetInvoiceByIdQuery query, CancellationToken cancellationToken);
    Task<PaginatedList<InvoicesDto>> GetInvoicesAsync(GetInvoicesQuery query, CancellationToken cancellationToken);
    Task UpdateInvoiceAsync(UpdateInvoiceCommand updateInvoiceCommand, CancellationToken cancellationToken);
    Task DeleteInvoiceAsync(DeleteInvoiceCommand deleteInvoiceCommand, CancellationToken cancellationToken);
}