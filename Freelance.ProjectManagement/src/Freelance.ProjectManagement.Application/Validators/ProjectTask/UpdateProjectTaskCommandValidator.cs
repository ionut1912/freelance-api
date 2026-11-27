using FluentValidation;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Domain.ValueObjects;

namespace Freelance.ProjectManagement.Application.Validators.ProjectTask;

public sealed class UpdateProjectTaskCommandValidator : AbstractValidator<UpdateProjectTaskCommand>
{
    public UpdateProjectTaskCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Project Id can not be empty");

        RuleFor(x => x.Title).MaximumLength(Domain.Entities.ProjectTask.TitleMaxLength);
        RuleFor(x => x.Description).MaximumLength(Domain.Entities.ProjectTask.DescriptionMaxLength);

        RuleFor(x => x.Status)
            .Must(ProjectTaskStatus.IsValid)
            .WithMessage($"Status can be only {string.Join(", ", ProjectTaskStatus.All.Select(s => $"'{s.Value}'"))}");

        RuleFor(x => x.Priority)
            .Must(ProjectTaskPriority.IsValid)
            .WithMessage($"Priority can be only {string.Join(", ", ProjectTaskPriority.All.Select(p => $"'{p.Value}'"))}");
    }
}
