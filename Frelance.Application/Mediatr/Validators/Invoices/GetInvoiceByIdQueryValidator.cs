using FluentValidation;
using Frelance.Application.Mediatr.Queries.Invoices;

namespace Frelance.Application.Mediatr.Validators.Invoices;

public class GetInvoiceByIdQueryValidator : AbstractValidator<GetInvoiceByIdQuery>
{
    public GetInvoiceByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }

}