using FluentValidation;
using Freelance.Application.Mediatr.Queries.Contracts;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Contracts;

[UsedImplicitly]
public class GetContractByIdQueryValidator : AbstractValidator<GetContractByIdQuery>
{
    public GetContractByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}