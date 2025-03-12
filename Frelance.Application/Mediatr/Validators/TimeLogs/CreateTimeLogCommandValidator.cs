using FluentValidation;
using Frelance.Application.Mediatr.Commands.TimeLogs;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.TimeLogs;

[UsedImplicitly]
public class CreateTimeLogCommandValidator : AbstractValidator<CreateTimeLogCommand>
{
    public CreateTimeLogCommandValidator()
    {
        RuleFor(x => x.CreateTimeLogRequest.TaskTitle).NotEmpty().MaximumLength(50);
        RuleFor(x => x.CreateTimeLogRequest.StartTime).NotEmpty();
        RuleFor(x => x.CreateTimeLogRequest.EndTime).NotEmpty();
    }
}