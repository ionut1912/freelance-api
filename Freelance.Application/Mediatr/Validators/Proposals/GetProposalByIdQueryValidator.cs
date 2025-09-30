using FluentValidation;
using Freelance.Application.Mediatr.Queries.Proposals;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Proposals;

[UsedImplicitly]
public class GetProposalByIdQueryValidator : AbstractValidator<GetProposalByIdQuery>
{
    public GetProposalByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}