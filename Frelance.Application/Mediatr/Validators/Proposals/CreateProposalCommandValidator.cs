using FluentValidation;
using Frelance.Application.Mediatr.Commands.Proposals;

namespace Frelance.Application.Mediatr.Validators.Proposals;

public class CreateProposalCommandValidator : AbstractValidator<CreateProposalCommand>
{
    public CreateProposalCommandValidator()
    {
        RuleFor(x => x.CreateProposalRequest.ProjectName).NotEmpty();
        RuleFor(x => x.CreateProposalRequest.ProposedBudget).NotEmpty();
    }
}