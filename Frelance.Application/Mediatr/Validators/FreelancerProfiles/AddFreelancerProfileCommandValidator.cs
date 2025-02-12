using FluentValidation;
using Frelance.Application.Mediatr.Commands.FreelancerProfiles;

namespace Frelance.Application.Mediatr.Validators.FreelancerProfiles;

public class AddFreelancerProfileCommandValidator : AbstractValidator<AddFreelancerProfileCommand>
{
    public AddFreelancerProfileCommandValidator()
    {
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.Bio).NotEmpty();
        RuleFor(x => x.Bio).MaximumLength(256);
        RuleFor(x => x.ProfileImage).NotEmpty();
        RuleFor(x => x.Skills).NotEmpty();
        RuleFor(x => x.ForeignLanguages).NotEmpty();
        RuleFor(x => x.Experience).NotEmpty();
        RuleFor(x => x.Rate).NotEmpty();
        RuleFor(x => x.Currency).NotEmpty();
        RuleFor(x => x.Rating).NotEmpty();
        RuleFor(x => x.PortfolioUrl).NotEmpty();

    }

}