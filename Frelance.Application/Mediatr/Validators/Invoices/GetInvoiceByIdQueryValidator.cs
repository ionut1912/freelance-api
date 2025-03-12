using FluentValidation;
using Frelance.Application.Mediatr.Queries.Invoices;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Invoices;

[UsedImplicitly]
public class GetInvoiceByIdQueryValidator : AbstractValidator<GetInvoiceByIdQuery>
{
    public GetInvoiceByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}