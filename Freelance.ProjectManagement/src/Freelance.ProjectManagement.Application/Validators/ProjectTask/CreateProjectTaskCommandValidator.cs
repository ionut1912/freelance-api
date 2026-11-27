using FluentValidation;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Domain.ValueObjects;

namespace Freelance.ProjectManagement.Application.Validators.ProjectTask;

public sealed class CreateProjectTaskCommandValidator : AbstractValidator<CreateProjectTaskCommand>
{
    public CreateProjectTaskCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Project Id can not be empty");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title can not be empty").MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description can not be empty").MaximumLength(500);

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status can not be empty")
            .Must(ProjectTaskStatus.IsValid)
            .WithMessage($"Status can be only {string.Join(", ", ProjectTaskStatus.All.Select(s => $"'{s.Value}'"))}");

        RuleFor(x => x.Priority)
            .NotEmpty().WithMessage("Priority can not be empty")
            .Must(ProjectTaskPriority.IsValid)
            .WithMessage($"Priority can be only {string.Join(", ", ProjectTaskPriority.All.Select(p => $"'{p.Value}'"))}");
    }
}
