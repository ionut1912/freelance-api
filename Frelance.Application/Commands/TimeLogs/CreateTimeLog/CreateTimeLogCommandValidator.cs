using FluentValidation;

namespace Frelance.Application.Commands.TimeLogs.CreateTimeLog;

public class CreateTimeLogCommandValidator:AbstractValidator<CreateTimeLogCommand>
{
    public CreateTimeLogCommandValidator()
    {
        RuleFor(x=>x.TaskTitle).NotEmpty().MaximumLength(50);
        RuleFor(x=>x.StartTime).NotEmpty();
        RuleFor(x=>x.EndTime).NotEmpty();
        RuleFor(x=>x.Date).NotEmpty();
    }
    
}