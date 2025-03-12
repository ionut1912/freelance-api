using FluentValidation;
using Frelance.Application.Mediatr.Commands.TimeLogs;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.TimeLogs;

[UsedImplicitly]
public class UpdateTimeLogCommandValidator : AbstractValidator<UpdateTimeLogCommand>
{
    public UpdateTimeLogCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UpdateTimeLogRequest.TotalHours).GreaterThan(0);
    }
}