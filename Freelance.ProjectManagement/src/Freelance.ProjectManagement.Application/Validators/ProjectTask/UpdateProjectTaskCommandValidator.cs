using FluentValidation;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;

namespace Freelance.ProjectManagement.Application.Validators.ProjectTask;

public class UpdateProjectTaskCommandValidator:AbstractValidator<UpdateProjectTaskCommand>
{
    public UpdateProjectTaskCommandValidator()
    {
        RuleFor(x => x.ProjectId)
          .NotEmpty()
          .WithMessage("Project Id can not be empty");

        RuleFor(x => x.Status)
            .Must(status => status == "New" || status == "InProgress" || status == "Review" || status == "Done")
            .WithMessage("Status can be only 'New',''InProgress,'Review','Done");

        RuleFor(x => x.Priority)
            .Must(priority => priority == "Low" || priority == "Medium" || priority == "High")
            .WithMessage("Priority can be only 'Low','Medium','High'");
    }
}
