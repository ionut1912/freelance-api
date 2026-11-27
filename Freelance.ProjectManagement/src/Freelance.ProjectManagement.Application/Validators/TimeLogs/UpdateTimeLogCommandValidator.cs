using FluentValidation;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;

namespace Freelance.ProjectManagement.Application.Validators.TimeLogs;

public sealed class UpdateTimeLogCommandValidator : AbstractValidator<UpdateTimeLogCommand>
{
    public UpdateTimeLogCommandValidator()
    {
        RuleFor(x => x.TaskId).NotEmpty().WithMessage("Task Id can not be empty");
        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("End Time must be after Start Time");
    }
}
