using FluentValidation;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;

namespace Freelance.ProjectManagement.Application.Validators.Projects;

public class UpdateProjectCommandValidator:AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be creater than 0");

        RuleFor(x => x.Currency)
            .Length(3)
            .WithMessage("Currency should be 3 characters length");
    }
}
