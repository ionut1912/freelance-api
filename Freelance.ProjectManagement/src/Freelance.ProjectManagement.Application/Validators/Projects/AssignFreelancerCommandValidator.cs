using FluentValidation;
using Freelance.ProjectManagement.Application.Mediatr.Projects.Commands;

namespace Freelance.ProjectManagement.Application.Validators.Projects;

public sealed class AssignFreelancerCommandValidator : AbstractValidator<AssignFreelancerCommand>
{
    public AssignFreelancerCommandValidator() =>
        RuleFor(x => x.FreelancerId).NotEmpty().WithMessage("Freelancer Id can not be empty");
}
