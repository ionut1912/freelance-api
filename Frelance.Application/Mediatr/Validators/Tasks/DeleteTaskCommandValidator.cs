using FluentValidation;
using Frelance.Application.Mediatr.Commands.Tasks;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Tasks;

[UsedImplicitly]
public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}