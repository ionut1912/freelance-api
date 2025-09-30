using FluentValidation;
using Freelance.Application.Mediatr.Commands.Projects;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Projects;

[UsedImplicitly]
public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UpdateProjectRequest.Title).MaximumLength(40);
        RuleFor(x => x.UpdateProjectRequest.Description).MaximumLength(500);
        RuleFor(x => x.UpdateProjectRequest.Budget).GreaterThan(0);
    }
}