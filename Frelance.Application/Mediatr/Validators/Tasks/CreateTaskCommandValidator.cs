using FluentValidation;
using Frelance.Application.Mediatr.Commands.Tasks;

namespace Frelance.Application.Mediatr.Validators.Tasks;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.ProjectTitle).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Priority).NotEmpty();

    }

}