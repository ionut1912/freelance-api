using FluentValidation;
using Freelance.Application.Mediatr.Commands.Invoices;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Invoices;

[UsedImplicitly]
public class DeleteInvoiceCommandValidator : AbstractValidator<DeleteInvoiceCommand>
{
    public DeleteInvoiceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}