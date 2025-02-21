using Frelance.Contracts.Requests.Invoices;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Invoices;

public record CreateInvoiceCommand(CreateInvoiceRequest CreateInvoiceRequest) : IRequest<Unit>;