using FluentValidation;
using Frelance.Application.Mediatr.Commands.Proposals;

namespace Frelance.Application.Mediatr.Validators.Proposals;

public class DeleteProposalCommandValidator : AbstractValidator<DeleteProposalCommand>
{
    public DeleteProposalCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}