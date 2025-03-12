using Frelance.Contracts.Requests.Invoices;
using JetBrains.Annotations;
using MediatR;

namespace Frelance.Application.Mediatr.Commands.Invoices;

[UsedImplicitly]
public record CreateInvoiceCommand(CreateInvoiceRequest CreateInvoiceRequest) : IRequest;