using FluentValidation;
using Freelance.ProjectManagement.Application.Mediatr.ProjectTasks.Commands;
using Freelance.ProjectManagement.Domain.ValueObjects;

namespace Freelance.ProjectManagement.Application.Validators.ProjectTask;

public class CreateProjectTaskCommandValidator:AbstractValidator<CreateProjectTaskCommand>
{
    public CreateProjectTaskCommandValidator()
    {
        RuleFor(x => x.ProjectId)
              .NotEmpty()
              .WithMessage("Project Id can not be empty");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title can not be empty");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description can not be emppty");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status can not be empty")
            .Must(status => status == "New" || status == "InProgress" || status == "Review" || status == "Done")
            .WithMessage("Status can be only 'New',''InProgress,'Review','Done");

        RuleFor(x => x.Priority)
            .NotEmpty()
            .WithMessage("Priority can not be empty")
            .Must(priority => priority == "Low" || priority == "Medium" || priority == "High")
            .WithMessage("Priority can be only 'Low','Medium','High'");

    }
}