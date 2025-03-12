using FluentValidation;
using Frelance.Application.Mediatr.Commands.Projects;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Projects;

[UsedImplicitly]
public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.CreateProjectRequest.Title).NotEmpty().MaximumLength(40);
        RuleFor(x => x.CreateProjectRequest.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.CreateProjectRequest.Deadline).NotEmpty();
        RuleFor(x => x.CreateProjectRequest.Technologies).NotEmpty();
        RuleFor(x => x.CreateProjectRequest.Budget).NotEmpty().GreaterThan(0);
    }
}