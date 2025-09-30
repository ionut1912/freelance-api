using Freelance.Application.Mediatr.Commands.Invoices;
using Freelance.Application.Repositories;
using MediatR;

namespace Freelance.Application.Mediatr.Handlers.Invoices;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(invoiceRepository, nameof(invoiceRepository));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
        _invoiceRepository = invoiceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        await _invoiceRepository.CreateInvoiceAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}