using FluentValidation;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;

namespace Freelance.ProjectManagement.Application.Validators.Projects;

public class CreateProjectCommandValidator:AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Project Title can not be empty");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description can not be empty");

        RuleFor(x => x.Deadline)
            .NotEmpty()
            .WithMessage("Deadline can not be empty");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be creater than 0");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage("Currency can not be empty")
            .Length(3)
            .WithMessage("Currency should be 3 characters length");

        RuleFor(x => x.Technologies)
            .NotEmpty()
            .WithMessage("Technologies can not be empty");
    }
}
