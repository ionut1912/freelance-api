using FluentValidation;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;

namespace Freelance.ProjectManagement.Application.Validators.TimeLogs;

public class CreateTimeLogCommandValidator:AbstractValidator<CreateTimeLogCommand>
{
    public CreateTimeLogCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty()
            .WithMessage("Task Id can not be empty");

        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("Start Time can not be empty");

        RuleFor(x => x.EndTime)
            .NotEmpty()
            .WithMessage("End Time can not be empty");
    }
}
