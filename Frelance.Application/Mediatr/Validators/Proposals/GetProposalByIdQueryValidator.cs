using FluentValidation;
using Frelance.Application.Mediatr.Queries.Proposals;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Proposals;

[UsedImplicitly]
public class GetProposalByIdQueryValidator : AbstractValidator<GetProposalByIdQuery>
{
    public GetProposalByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}