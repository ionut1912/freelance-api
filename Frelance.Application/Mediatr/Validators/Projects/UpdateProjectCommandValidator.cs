using FluentValidation;
using Frelance.Application.Mediatr.Commands.Projects;

namespace Frelance.Application.Mediatr.Validators.Projects;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).MaximumLength(40);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Budget).GreaterThan(0);

    }

}