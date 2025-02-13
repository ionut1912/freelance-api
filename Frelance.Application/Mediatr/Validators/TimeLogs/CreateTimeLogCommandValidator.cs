using FluentValidation;
using Frelance.Application.Mediatr.Commands.TimeLogs;

namespace Frelance.Application.Mediatr.Validators.TimeLogs;

public class CreateTimeLogCommandValidator : AbstractValidator<CreateTimeLogCommand>
{
    public CreateTimeLogCommandValidator()
    {
        RuleFor(x => x.CreateTimeLogRequest.TaskTitle).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CreateTimeLogRequest.StartTime).NotEmpty();
        RuleFor(x => x.CreateTimeLogRequest.EndTime).NotEmpty();
    }

}