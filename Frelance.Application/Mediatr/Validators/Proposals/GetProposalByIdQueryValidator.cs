using FluentValidation;
using Frelance.Application.Mediatr.Queries.Proposals;

namespace Frelance.Application.Mediatr.Validators.Proposals;

public class GetProposalByIdQueryValidator : AbstractValidator<GetProposalByIdQuery>
{
    public GetProposalByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}