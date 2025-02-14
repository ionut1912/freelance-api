using FluentValidation;
using Frelance.Application.Mediatr.Commands.Invoices;

namespace Frelance.Application.Mediatr.Validators.Invoices;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(x => x.CreateInvoiceRequest.ProjectName).NotEmpty();
        RuleFor(x => x.CreateInvoiceRequest.ClientName).NotEmpty();
        RuleFor(x => x.CreateInvoiceRequest.Amount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.CreateInvoiceRequest.InvoiceFile).NotEmpty();
        RuleFor(x => x.CreateInvoiceRequest.InvoiceFile)
            .Must(file =>
                file != null &&
                (file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase)
                 || file.ContentType.Equals("application/msword", StringComparison.OrdinalIgnoreCase)
                 || file.ContentType.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document", StringComparison.OrdinalIgnoreCase)))
            .WithMessage("Contract file must be a PDF or Word document.");
    }

}