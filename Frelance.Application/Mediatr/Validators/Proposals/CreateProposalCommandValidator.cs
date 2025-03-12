using FluentValidation;
using Frelance.Application.Mediatr.Commands.Proposals;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Proposals;

[UsedImplicitly]
public class CreateProposalCommandValidator : AbstractValidator<CreateProposalCommand>
{
    public CreateProposalCommandValidator()
    {
        RuleFor(x => x.CreateProposalRequest.ProjectName).NotEmpty();
        RuleFor(x => x.CreateProposalRequest.ProposedBudget).NotEmpty();
    }
}