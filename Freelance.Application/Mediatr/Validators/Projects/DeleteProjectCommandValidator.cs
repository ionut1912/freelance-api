using FluentValidation;
using Freelance.Application.Mediatr.Commands.Projects;
using JetBrains.Annotations;

namespace Freelance.Application.Mediatr.Validators.Projects;

[UsedImplicitly]
public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}