using FluentValidation;
using Freelance.Application.Mediatr.Queries.Invoices;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Invoices;

[UsedImplicitly]
public class GetInvoiceByIdQueryValidator : AbstractValidator<GetInvoiceByIdQuery>
{
    public GetInvoiceByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}