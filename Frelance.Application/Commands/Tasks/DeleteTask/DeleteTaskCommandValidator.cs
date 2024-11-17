using FluentValidation;

namespace Frelance.API.Frelance.Application.Commands.Tasks.DeleteTask;

public class DeleteTaskCommandValidator:AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
    
}