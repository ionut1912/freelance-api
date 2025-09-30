using FluentValidation;
using Freelance.Application.Mediatr.Commands.TimeLogs;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.TimeLogs;

[UsedImplicitly]
public class DeleteTimeLogCommandValidator : AbstractValidator<DeleteTimeLogCommand>
{
    public DeleteTimeLogCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}