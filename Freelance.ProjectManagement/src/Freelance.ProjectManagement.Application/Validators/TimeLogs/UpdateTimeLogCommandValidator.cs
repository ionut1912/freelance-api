using FluentValidation;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Domain.ValueObjects;

namespace Freelance.ProjectManagement.Application.Validators.TimeLogs;

public sealed class UpdateTimeLogCommandValidator : AbstractValidator<UpdateTimeLogCommand>
{
    public UpdateTimeLogCommandValidator()
    {
        RuleFor(x => x.TaskId).NotEmpty().WithMessage("Task Id can not be empty");
        RuleFor(x => x.StartTime).NotEmpty().WithMessage("Start Time can not be empty");
        RuleFor(x => x.EndTime)
            .Must((command, end) => UtcDateTime.From(end) > UtcDateTime.From(command.StartTime)).WithMessage("End Time must be after Start Time");
    }
}
