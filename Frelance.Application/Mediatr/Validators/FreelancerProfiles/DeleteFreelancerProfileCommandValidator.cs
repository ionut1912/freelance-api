using FluentValidation;
using Frelance.Application.Mediatr.Commands.FreelancerProfiles;

namespace Frelance.Application.Mediatr.Validators.FreelancerProfiles;

public class DeleteFreelancerProfileCommandValidator : AbstractValidator<DeleteFreelancerProfileCommand>
{
    public DeleteFreelancerProfileCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }

}