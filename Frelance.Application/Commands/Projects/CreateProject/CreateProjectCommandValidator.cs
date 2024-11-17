using FluentValidation;

namespace Frelance.API.Frelance.Application.Commands.Projects.CreateProject;

public class CreateProjectCommandValidator:AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(40);
        RuleFor(x=>x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Deadline).NotEmpty();
        RuleFor(x=>x.Technologies).NotEmpty();
        RuleFor(x => x.Budget).NotEmpty();
    }   
}