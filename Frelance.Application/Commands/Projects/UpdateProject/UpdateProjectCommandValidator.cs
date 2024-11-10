using FluentValidation;

namespace Frelance.API.Frelance.Application.Commands.Projects.UpdateProject;

public class UpdateProjectCommandValidator:AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x=>x.Id).NotEmpty();
        RuleFor(x => x.Title).MaximumLength(40);
        RuleFor(x=>x.Description).MaximumLength(500);
    }
    
}