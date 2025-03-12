using FluentValidation;
using Frelance.Application.Mediatr.Commands.TimeLogs;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.TimeLogs;

[UsedImplicitly]
public class DeleteTimeLogCommandValidator : AbstractValidator<DeleteTimeLogCommand>
{
    public DeleteTimeLogCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}