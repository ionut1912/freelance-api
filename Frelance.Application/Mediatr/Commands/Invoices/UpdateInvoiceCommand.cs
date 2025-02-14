using Frelance.Contracts.Requests.Invoices;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Invoices;

public record UpdateInvoiceCommand(int Id, UpdateInvoiceRequest UpdateInvoiceRequest) : IRequest<Unit>;