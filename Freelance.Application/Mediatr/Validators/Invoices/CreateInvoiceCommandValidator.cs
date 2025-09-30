using FluentValidation;
using Freelance.Application.Mediatr.Commands.Invoices;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Invoices;

[UsedImplicitly]
public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x.CreateInvoiceRequest.ProjectName).NotEmpty();
        RuleFor(x => x.CreateInvoiceRequest.ClientName).NotEmpty();
        RuleFor(x => x.CreateInvoiceRequest.Amount).NotEmpty().GreaterThan(0);
    }
}