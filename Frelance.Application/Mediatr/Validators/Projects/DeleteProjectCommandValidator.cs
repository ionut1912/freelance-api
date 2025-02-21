using FluentValidation;
using Frelance.Application.Mediatr.Commands.Projects;

namespace Frelance.Application.Mediatr.Validators.Projects;

public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}