using FluentValidation;
using Frelance.Application.Mediatr.Commands.Tasks;

namespace Frelance.Application.Mediatr.Validators.Tasks;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.CreateProjectTaskRequest.ProjectTitle).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CreateProjectTaskRequest.Title).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CreateProjectTaskRequest.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.CreateProjectTaskRequest.Priority).NotEmpty();
    }
}