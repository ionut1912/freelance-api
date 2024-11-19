using FluentValidation;

namespace Frelance.Application.Commands.Tasks.CreateTask;

public class CreateTaskCommandValidator:AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.ProjectTitle).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
        RuleFor(x=>x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x=>x.Priority).NotEmpty();

    }
    
}