using MediatR;

namespace Freelance.Application.Mediatr.Commands.Invoices;

public record DeleteInvoiceCommand(int Id) : IRequest;