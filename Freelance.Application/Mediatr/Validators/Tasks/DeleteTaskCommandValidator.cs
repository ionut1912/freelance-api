using FluentValidation;
using Freelance.Application.Mediatr.Commands.Tasks;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Tasks;

[UsedImplicitly]
public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}