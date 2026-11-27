using Freelance.ProjectManagement.Domain.ValueObjects;
using Freelance.ProjectManagement.Domain.Entities;
using FluentValidation;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;

namespace Freelance.ProjectManagement.Application.Validators.Projects;

public sealed class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {

        // Title, description and technologies are optional: omitted values are left unchanged.
        RuleFor(x => x.Title).MaximumLength(Project.TitleMaxLength);
        RuleFor(x => x.Description).MaximumLength(Project.DescriptionMaxLength);
        RuleForEach(x => x.Technologies).NotEmpty().MaximumLength(ProjectTechnology.MaxLength);

        RuleFor(x => x.Deadline)
            .NotEmpty()
            .WithMessage("Deadline can not be empty");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage("Currency can not be empty")
            .Length(Project.CurrencyLength)
            .WithMessage("Currency should be 3 characters length");
    }
}
