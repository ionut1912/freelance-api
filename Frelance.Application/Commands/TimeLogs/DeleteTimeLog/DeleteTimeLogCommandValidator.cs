using FluentValidation;

namespace Frelance.Application.Commands.TimeLogs.DeleteTimeLog;

public class DeleteTimeLogCommandValidator:AbstractValidator<DeleteTimeLogCommand>
{
    public DeleteTimeLogCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
    
}