using MediatR;

namespace Frelance.Application.Mediatr.Commands.Invoices;

public record DeleteInvoiceCommand(int Id):IRequest<Unit>;