using FluentValidation;
using Frelance.Application.Mediatr.Commands.TimeLogs;

namespace Frelance.Application.Mediatr.Validators.TimeLogs;

public class DeleteTimeLogCommandValidator : AbstractValidator<DeleteTimeLogCommand>
{
    public DeleteTimeLogCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }

}