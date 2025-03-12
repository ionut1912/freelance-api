using FluentValidation;
using Frelance.Application.Mediatr.Commands.Projects;
using JetBrains.Annotations;

namespace Frelance.Application.Mediatr.Validators.Projects;

[UsedImplicitly]
public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}