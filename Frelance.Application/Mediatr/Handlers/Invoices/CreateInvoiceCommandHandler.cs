using Frelance.Application.Mediatr.Commands.Invoices;
using Frelance.Application.Repositories;
using MediatR;

namespace Frelance.Application.Mediatr.Handlers.Invoices;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Unit>
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

    public async Task<Unit> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        await _invoiceRepository.CreateInvoiceAsync(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}