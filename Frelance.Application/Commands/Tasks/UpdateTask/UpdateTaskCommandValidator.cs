using FluentValidation;

namespace Frelance.Application.Commands.Tasks.UpdateTask;

public class UpdateTaskCommandValidator:AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Title).MaximumLength(50);
        RuleFor(x=>x.Description).MaximumLength(500);
    }
    
}