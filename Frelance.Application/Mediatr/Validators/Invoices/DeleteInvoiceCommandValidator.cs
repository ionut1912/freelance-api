using FluentValidation;
using Frelance.Application.Mediatr.Commands.Invoices;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Invoices;

[UsedImplicitly]
public class DeleteInvoiceCommandValidator : AbstractValidator<DeleteInvoiceCommand>
{
    public DeleteInvoiceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}