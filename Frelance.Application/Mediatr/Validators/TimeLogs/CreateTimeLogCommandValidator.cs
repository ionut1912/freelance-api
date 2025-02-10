using FluentValidation;
using Frelance.Application.Mediatr.Commands.TimeLogs;

namespace Frelance.Application.Mediatr.Validators.TimeLogs;

public class CreateTimeLogCommandValidator : AbstractValidator<CreateTimeLogCommand>
{
    public CreateTimeLogCommandValidator()
    {
        RuleFor(x => x.TaskTitle).NotEmpty().MaximumLength(50);
        RuleFor(x => x.StartTime).NotEmpty();
        RuleFor(x => x.EndTime).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
    }

}