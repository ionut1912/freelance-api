using Freelance.Contracts.Requests.Invoices;
using JetBrains.Annotations;
using MediatR;

namespace Freelance.Application.Mediatr.Commands.Invoices;

[UsedImplicitly]
public record CreateInvoiceCommand(CreateInvoiceRequest CreateInvoiceRequest) : IRequest;