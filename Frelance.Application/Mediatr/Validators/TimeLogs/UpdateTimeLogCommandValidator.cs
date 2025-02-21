using FluentValidation;
using Frelance.Application.Mediatr.Commands.TimeLogs;

namespace Frelance.Application.Mediatr.Validators.TimeLogs;

public class UpdateTimeLogCommandValidator : AbstractValidator<UpdateTimeLogCommand>
{
    public UpdateTimeLogCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UpdateTimeLogRequest.TotalHours).GreaterThan(0);
    }
}