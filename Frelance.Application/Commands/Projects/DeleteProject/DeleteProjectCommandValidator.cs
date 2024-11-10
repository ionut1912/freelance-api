using FluentValidation;

namespace Frelance.API.Frelance.Application.Commands.Projects.DeleteProject;

public class DeleteProjectCommandValidator:AbstractValidator<DeleteProjectCommand>
{
    public DeleteProjectCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
    
}