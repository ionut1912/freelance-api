using FluentValidation;
using Frelance.Application.Mediatr.Queries.Contracts;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Contracts;

[UsedImplicitly]
public class GetContractByIdQueryValidator : AbstractValidator<GetContractByIdQuery>
{
    public GetContractByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}