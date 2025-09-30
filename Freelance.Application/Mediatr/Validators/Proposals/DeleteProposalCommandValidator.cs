using FluentValidation;
using Freelance.Application.Mediatr.Commands.Proposals;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Proposals;

[UsedImplicitly]
public class DeleteProposalCommandValidator : AbstractValidator<DeleteProposalCommand>
{
    public DeleteProposalCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}