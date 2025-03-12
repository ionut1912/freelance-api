using Frelance.Contracts.Requests.Invoices;
using JetBrains.Annotations;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Invoices;

[UsedImplicitly]
public record UpdateInvoiceCommand(int Id, UpdateInvoiceRequest UpdateInvoiceRequest) : IRequest;