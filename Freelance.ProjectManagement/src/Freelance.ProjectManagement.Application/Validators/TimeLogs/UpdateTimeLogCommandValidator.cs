using FluentValidation;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;

namespace Freelance.ProjectManagement.Application.Validators.TimeLogs;

public class UpdateTimeLogCommandValidator:AbstractValidator<UpdateTimeLogCommand>
{
    public UpdateTimeLogCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty()
            .WithMessage("Task Id can not be empty");
    }
}
