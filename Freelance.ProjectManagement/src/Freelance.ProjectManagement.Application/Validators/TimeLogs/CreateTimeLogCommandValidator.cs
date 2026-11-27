using FluentValidation;
using Freelance.ProjectManagement.Application.Mediatr.TimeLogs.Commands;
using Freelance.ProjectManagement.Domain.ValueObjects;

namespace Freelance.ProjectManagement.Application.Validators.TimeLogs;

public sealed class CreateTimeLogCommandValidator : AbstractValidator<CreateTimeLogCommand>
{
    public CreateTimeLogCommandValidator()
    {
        RuleFor(x => x.TaskId).NotEmpty().WithMessage("Task Id can not be empty");
        RuleFor(x => x.StartTime).NotEmpty().WithMessage("Start Time can not be empty");
        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End Time can not be empty")
            .Must((command, end) => UtcDateTime.From(end) > UtcDateTime.From(command.StartTime)).WithMessage("End Time must be after Start Time");
    }
}
