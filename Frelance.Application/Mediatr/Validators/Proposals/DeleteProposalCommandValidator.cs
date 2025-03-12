using FluentValidation;
using Frelance.Application.Mediatr.Commands.Proposals;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Proposals;

[UsedImplicitly]
public class DeleteProposalCommandValidator : AbstractValidator<DeleteProposalCommand>
{
    public DeleteProposalCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}