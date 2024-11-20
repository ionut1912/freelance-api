using FluentValidation;

namespace Frelance.Application.Commands.TimeLogs.UpdateTimeLog;

public class UpdateTimeLogCommandValidator:AbstractValidator<UpdateTimeLogCommand>
{
    public UpdateTimeLogCommandValidator()
    {
        RuleFor(x=>x.Id).NotEmpty();
        RuleFor(x=>x.TotalHours).GreaterThan(0);
    }
    
}