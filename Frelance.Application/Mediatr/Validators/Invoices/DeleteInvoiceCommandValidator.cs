using FluentValidation;
using Frelance.Application.Mediatr.Commands.Invoices;

namespace Frelance.Application.Mediatr.Validators.Invoices;

public class DeleteInvoiceCommandValidator : AbstractValidator<DeleteInvoiceCommand>
{
    public DeleteInvoiceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}