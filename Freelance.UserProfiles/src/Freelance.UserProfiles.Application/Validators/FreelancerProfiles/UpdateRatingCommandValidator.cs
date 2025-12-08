using FluentValidation;
using Freelance.UserProfiles.Application.Mediatr.FreelancerProfiles.Commands;

namespace Freelance.UserProfiles.Application.Validators.FreelancerProfiles;

public class UpdateRatingCommandValidator : AbstractValidator<UpdateFreelancerRatingCommand>
{
    public UpdateRatingCommandValidator()
    {
        RuleFor(x => x.Rating).NotEmpty().WithMessage("Rating is required");
    }
}